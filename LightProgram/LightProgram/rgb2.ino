
#include <EEPROM.h>

typedef struct isr_data_s {
  unsigned long time;
  unsigned counter;
  unsigned copy_counter;
  char state;
} isr_data_t;

volatile isr_data_t isr[2];
const unsigned long debounce_delay = 50;
const unsigned program_size = 128;
const unsigned num_programs = 8;
unsigned program_bytes[program_size];
unsigned program_counter = 0;
unsigned current_program = 0;
unsigned free_run = 1;
unsigned char r, g, b;
unsigned char r1, g1, b1;


////////////////////////////////////
// Set the 5082 hex digit display
void set_display (unsigned char i)
{
  // D4 -> 5082 pin 4 -> blank control was D8
  // D5 -> 5082 pin 3 -> input 8   was D7 
  // D6 -> 5082 pin 2 -> input 4   was D6
  // D7 -> 5082 pin 1 -> input 2   was D5
  // D8 -> 5082 pin 8 -> input 1   was D4
  // GND -> 5082 pin 5 -> latch
  // GND -> 5082 pin 6 -> GND
  // 5V  -> 5082 pin 7 -> VCC

  // blank display
  digitalWrite (4, 1);
  if (i < 0x10) {
    digitalWrite (5, (i >> 3) & 1);
    digitalWrite (6, (i >> 2) & 1);
    digitalWrite (7, (i >> 1) & 1);
    digitalWrite (8, (i >> 0) & 1);
    digitalWrite (4, 0); // unblank
  }
}

////////////////////////////////////
// Interrupt service routine used for buttons
void isr_fn (void)
{
  int pin;
  for (pin = 0; pin <= 1; pin++) {
    char state = digitalRead (pin + 2);
  
    if (state != isr[pin].state) {
      // state has changed
      if ((millis () - isr[pin].time) > debounce_delay) {
        // state was stable
        if (!state) {
          // state changed to 0, register change
          isr[pin].counter ++;
        }      
      }
      isr[pin].time = millis ();
      isr[pin].state = state;
    }
  }
}

////////////////////////////////////
// Get a serial byte: block, and then remove it from the FIFO
unsigned char get_serial_byte ()
{
  while (!Serial.available()) {}
  return Serial.read();
}

////////////////////////////////////
// Get a key press (non-blocking) and remove it from the isr data structure
// Return 1 (up key pressed), -1 (down key pressed), or 0 (neither)
signed char get_key_press ()
{
  int i;
  for (i = 0; i < 2; i++) {
    unsigned c = isr[i].counter;
    if (c != isr[i].copy_counter) {
      isr[i].copy_counter = c;
      return (i * 2) - 1;
    }
  }
  return 0;
}

////////////////////////////////////
// Return 1 if a key press has been received since the last call to get_key_press()
unsigned char has_key_press ()
{
  signed char i;
  for (i = 0; i < 2; i++) {
    unsigned c = isr[i].counter;
    if (c != isr[i].copy_counter) {
      return 1;
    }
  }
  return 0;
}

////////////////////////////////////
// Get next program byte and advance program counter
unsigned char get_program_byte ()
{
  unsigned char b = program_bytes[program_counter];
  program_counter ++;
  if (program_counter >= program_size) {
    program_counter = 0;
  }
  return b;
}

////////////////////////////////////
// Load a program from EEPROM
void load_program (void)
{
  unsigned i, j;

  j = program_size * current_program;
  for (i = 0; i < program_size; i++) {
    program_bytes[i] = EEPROM.read (i + j);
  }
  program_counter = 0;
}

////////////////////////////////////
// Save a program to EEPROM
void save_program (void)
{
  unsigned i, j;

  j = program_size * current_program;
  for (i = 0; i < program_size; i++) {
    EEPROM.update (i + j, program_bytes[i]);
  }
}

////////////////////////////////////
// Set colour output to (r, g, b)
void set_colour (void)
{
  analogWrite(9, b);
  analogWrite(10, r);
  analogWrite(11, g);
}

unsigned char linear (long s, long t, long p, long q)
{
  // clamped to 0 and 255
  long output = s + (((t - s) * p) / q);

  if (output >= 255) {
    return 255;
  } else if (output <= 0) {
    return 0;
  } else {
    return (unsigned char) output;
  }
}

////////////////////////////////////
// Transition from (r, g, b) to (r1, g1, b1) in the specified time interval
void transition (unsigned interval)
{
  long rS, gS, bS;
  long rT, gT, bT;
  long tS, tD, tI;

  rS = r;  gS = g;  bS = b;
  rT = r1; gT = g1; bT = b1;
  tS = millis ();
  tI = interval;
  tD = 0;
  while (tD < tI) {
    r = linear (rS, rT, tD, tI);
    g = linear (gS, gT, tD, tI);
    b = linear (bS, bT, tD, tI);
    set_colour ();
    if (has_key_press () || Serial.available ()) {
      // early exit, go back to main loop
      break;
    }
    delay (5);
    tD = millis () - tS;
  }
  // ensure that we switch to target colour
  r = r1; g = g1; b = b1;
  set_colour ();
}

////////////////////////////////////
// PC control
void pc_mode ()
{
  unsigned i, tmp;

  switch (get_serial_byte()) {
    case 'Q':
      // Query - does nothing other than:
      Serial.write('K');
      break;
    case 'q':
      // Query - does nothing other than:
      Serial.write('q');
      break;
    case 'D':
      // Set Display
      set_display(get_serial_byte());
      Serial.write('K');
      break;
    case 'R':
      // Run current program
      Serial.write('K');
      program_counter = 0;
      free_run = 1;
      break;
    case 'L':
      // load a new program from EEPROM
      current_program = get_serial_byte ();
      if (current_program >= num_programs) {
        current_program = 0;
        Serial.write('?');
      } else {
        set_display (0xf);
        load_program ();
        Serial.write('K');
        set_display (0xc);
      }
      break;
    case 'S':
      // save current program in EEPROM
      current_program = get_serial_byte ();
      if (current_program >= num_programs) {
        current_program = 0;
        Serial.write('?');
      } else {
        set_display (0xf);
        save_program ();
        Serial.write('K');
        set_display (0xc);
      }
      break;
    case 'C':
      // Set current colour (manual control)
      r = get_serial_byte();
      g = get_serial_byte();
      b = get_serial_byte();
      set_colour ();
      Serial.write('K');
      break;
    case 'B':
      // Blackout
      r = g = b = 0;
      set_colour ();
      Serial.write('K');
      break;
    case 'c':
      // Get current colour
      Serial.write('K');
      Serial.write(r);
      Serial.write(g);
      Serial.write(b);
      break;
    case 'T':
      // Fast transition to colour (max time 255ms)
      r1 = get_serial_byte();
      g1 = get_serial_byte();
      b1 = get_serial_byte();
      tmp = get_serial_byte ();
      Serial.write('K');
      set_display (0xb);
      transition (tmp);
      set_display (0xc);
      break;
    case 't':
      // Slow transition to colour (max time 65535ms)
      r1 = get_serial_byte();
      g1 = get_serial_byte();
      b1 = get_serial_byte();
      tmp = (unsigned) get_serial_byte () << (unsigned) 8;
      tmp |= (unsigned) get_serial_byte ();
      Serial.write('K');
      set_display (0xb);
      transition (tmp);
      set_display (0xc);
      break;
    case 'm':
      // Get current program
      Serial.write('K');
      for (i = 0; i < program_size; i++) {
        Serial.write(program_bytes[i]);
      }
      break;
    case 'M':
      // Set current program
      set_display (0xd);
      for (i = 0; i < program_size; i++) {
        program_bytes[i] = get_serial_byte();
      }
      set_display (0xc);
      program_counter = 0;
      Serial.write('K');
      break;
    case '\0':
    case '\n':
    case '\r':
    case ' ':
      // Null/whitespace bytes ignored
      break;
    default:
      // Anything else is an error
      set_display(0xa);
      Serial.write('?');
      break;
  }
}

////////////////////////////////////
// Main program
void loop() 
{
  unsigned tmp, undo;
  signed char key;

  // Check for PC mode (commands from the PC)
  if (!free_run) {
    pc_mode ();
    return;
  }

  // Poll for serial data (will be interpreted as a command from the PC)
  if (Serial.available()) {
    set_display (0xc);
    free_run = 0;
    pc_mode ();
    return;
  }

  // Look for key presses (changes program)
  key = get_key_press();
  if (key != 0) {
    set_display (0xf);
    current_program += key;
    current_program %= num_programs;
    load_program ();
    set_display (current_program);
    return;
  }

  // Run the next program instruction
  undo = program_counter;
  switch (get_program_byte()) {
    case 'R':
      // Re-run current program
      program_counter = 0;
      break;
    case 'D':
      // Set Display
      set_display(get_program_byte());
      break;
    case 'B':
      // Blackout
      r = g = b = 0;
      set_colour ();
      break;
    case 'C':
      // Set current colour
      r = get_program_byte();
      g = get_program_byte();
      b = get_program_byte();
      set_colour ();
      break;
    case 'T':
      // Transition to colour
      r1 = get_program_byte();
      g1 = get_program_byte();
      b1 = get_program_byte();
      transition (get_program_byte ());
      break;
    case 't':
      // Slow transition to colour (max time 65535ms)
      r1 = get_program_byte();
      g1 = get_program_byte();
      b1 = get_program_byte();
      tmp = (unsigned) get_program_byte () << (unsigned) 8;
      tmp |= (unsigned) get_program_byte ();
      transition (tmp);
      break;
    default:
      // Illegal opcode; program crashes (freezes at same location)
      set_display(0xe);
      program_counter = undo;
      break;
  }
}


////////////////////////////////////
// Entry point
void setup() 
{
  unsigned i;
  Serial.begin (19200);
  set_display (0x10);
  for (i = 4; i <= 11; i++) {
    pinMode (i, OUTPUT);
  }
  for (i = 0; i <= 1; i++) {
    pinMode (i + 2, INPUT_PULLUP);
    isr[i].time = 0;
    isr[i].counter = 0;
    isr[i].state = 0;
    isr[i].copy_counter = 0;
    attachInterrupt (i, isr_fn, CHANGE);
  }
  program_counter = 0;
  current_program = 0;
  r = g = b = 0;
  r1 = g1 = b1 = 0;
  free_run = 1;
  set_display (0xf);
  load_program ();
  set_display (current_program);
}

