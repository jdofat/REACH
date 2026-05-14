import board
import analogio
import time

# Initialize the flex sensor on A0
flex = analogio.AnalogIn(board.A0)

while True:
    voltage = (flex.value * 3.3) / 65535  # Convert 16-bit ADC to voltage
    print(voltage)                         # Send value over USB serial
    time.sleep(0.05)                       # ~20 readings/sec
