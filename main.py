readLine = ""

def on_data_received():
    global readLine
    serial.write_line(serial.read_line())
    readLine = serial.read_line().char_at(0)
serial.on_data_received(serial.delimiters(Delimiters.CARRIAGE_RETURN),
    on_data_received)

def on_button_pressed_a():
    serial.write_line("hi")
input.on_button_pressed(Button.A, on_button_pressed_a)

def on_forever():
    global readLine
    if readLine == "u":
        basic.show_leds("""
            . . # . .
                        . # # # .
                        # . # . #
                        . . # . .
                        . . # . .
        """)
        basic.pause(200)
        readLine = ""
        basic.show_leds("""
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        """)
    elif readLine == "d":
        basic.show_leds("""
            . . # . .
                        . . # . .
                        # . # . #
                        . # # # .
                        . . # . .
        """)
        basic.pause(200)
        readLine = ""
        basic.show_leds("""
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        """)
    elif readLine == "l":
        basic.show_leds("""
            . . # . .
                        . # . . .
                        # # # # #
                        . # . . .
                        . . # . .
        """)
        basic.pause(200)
        readLine = ""
        basic.show_leds("""
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        """)
    elif readLine == "r":
        basic.show_leds("""
            . . # . .
                        . . . # .
                        # # # # #
                        . . . # .
                        . . # . .
        """)
        basic.pause(200)
        readLine = ""
        basic.show_leds("""
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        """)
    elif readLine == "b":
        basic.show_string("B")
        basic.pause(200)
        readLine = ""
        basic.show_leds("""
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        """)
    elif readLine == "a":
        basic.show_string("A")
        basic.pause(200)
        readLine = ""
        basic.show_leds("""
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        """)
    elif readLine == "s":
        basic.show_string("START")
        basic.pause(200)
        readLine = ""
        basic.show_leds("""
            . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
                        . . . . .
        """)
    serial.write_line(str(input.light_level()))
    basic.pause(100)
basic.forever(on_forever)
