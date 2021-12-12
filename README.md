# Midi Keyboard Soundboard
Simple WPF desktop app that configures a midi keyboard (or midi pad controller) to be a soundboard. Tested with an AKAI LPD 8.

Feel free to raise issues or create pull requests.

## Motivation
The motivation for me to create this project is that I couldn't find any software (except https://github.com/astromme/midi-keyboard-sounds) that allows me to simply configure sounds with my midi pad controller. There is a lot of software out there that allows me to do this (like ableton), except this comes with a lot more (unused) functionalities. That means that I only use the tip of the iceberg.

## Download
See https://github.com/JennoVink/MidiKeyboardSoundboard/releases

## Play sounds through the microphone
- Install [VBCable](https://vb-audio.com/Cable/index.htm)
- Open your sound settings and select "App volume and device preferences"
- Set the Output device for the Soundboard Application to "CABLE Input"
- Change your microphone in the application in which you need the soundboard to "CABLE Output"

### Soundboard + voice:
- Open the Sound Control Panel and go to Recording
- Select your normal microphone and go to the properties
- Select lists and activate "Listen to this device"
- Change the playback device to "CABLE Input"
