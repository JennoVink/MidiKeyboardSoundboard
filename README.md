# Midi Keyboard Soundboard (also known as: Arjen Lubach soundboard, zondag met Lubach soundboard)
Simple WPF desktop app that configures a midi keyboard (or midi pad controller) to be a soundboard. Tested with an AKAI LPD 8.

Feel free to raise issues or create pull requests.

## Demo
[![Video of demo of the midi keyboard soundboard](https://img.youtube.com/vi/nENmCYmhjuY/0.jpg)](https://www.youtube.com/watch?v=nENmCYmhjuY)

## Motivation
The motivation for me to create this project is that I couldn't find any software (except https://github.com/astromme/midi-keyboard-sounds) that allows me to simply configure sounds with my midi pad controller. There is a lot of software out there that allows me to do this (like ableton), except this comes with a lot more (unused) functionalities. That means that I only use the tip of the iceberg.

## Download
Checkout the [releases!](https://github.com/JennoVink/MidiKeyboardSoundboard/releases/)

## Use with Discord: play soundboard sounds through the microphone (while streaming) (credits to: @HypedDomi)
- Install [VBCable](https://vb-audio.com/Cable/index.htm)
- Open your sound settings and select "App volume and device preferences"
- Set the Output device for the Soundboard Application to "CABLE Input"
- Change your microphone in the application you need (e.g. Twitch, discord, etc) to: "CABLE Output"

### Use both soundboard and microphone at once:
- Open the Sound Control Panel and go to Recording
- Select your normal microphone and go to the properties
- Select lists and activate "Listen to this device"
- Change the playback device to "CABLE Input"
