# Rubiks View
This application is a 3D Rubiks Cube oriented by faces (front, back, right,
left, up and down) with support to be controlled over network.

The main purpose for this application is to work as a front-end application
to simulate rotations on the rubiks cube.

# GUI
You will be able to rotate the faces using the corresponding keys `F`, `B`, `R`,
`L`, `U` and `D`) for a clockwise rotation (plus `shift` for a counter clockwise
rotation).

# UDP Controller

## Why UDP!?
The intend for this application is to provide a easy front-end for Rubiks Cube
solving studies (as mentioned before). So, our main goal is to provide a very
easy way of communication. In many languages you will be able to implement the
communication with few lines of code.

## Sending Messages

### Terminal (Linux like systems)

For testing purposes, might be useful sending quick messages straight from your
terminal. To do so, you can use a set of tools for sending packages through UDP.
Here, we will show you an easy way to do it using the `nc` (Netcat) command:

```
$ echo '<your JSON here>' | nc localhost -u 4572
```

## Messages

### Reset
Reset all faces to the default colour.

**Parameters**:

There is no parameters for the reset command.

**Example**:
```
{"name":"reset","params":{}}
```

### Rotation
Rotate a face. The system will enqueue all rotation commands and execute them
in the same order they arrive on the server.

**Parameters**:

    *face*: `(required)` The face that will be rotated. Possible values: (
        `front`, `back`, `right`, `left`, `up`, `down`)


    *times*: `(optional)` `default: 1` Amount of turning. This value can also be
        negative (meaning that the rotation will be counter-clockwise).


    *duration*: `(optional)` `default: 0.5` Time that the turn will take (just
        for a visual effect).

**Example**:
```
{"name":"rotate","params":{"face":"back", "times": 1, "duration": 0.5}}
```

## Downloads
Goto to [releases](https://github.com/jamillosantos/rubiks-view/releases) page
and check all binary distributions available.
