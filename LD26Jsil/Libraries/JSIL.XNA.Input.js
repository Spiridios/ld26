"use strict";

if (typeof (JSIL) === "undefined")
  throw new Error("JSIL.Core required");

if (typeof ($jsilxna) === "undefined")
  throw new Error("JSIL.XNACore required");

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.Keyboard", function ($) {
  var getStateImpl = function (playerIndex) {
    var keys = JSIL.Host.getHeldKeys();
    return new Microsoft.Xna.Framework.Input.KeyboardState(keys);
  };

  $.Method({Static:true , Public:true }, "GetState", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.KeyboardState"), [], [])), 
    getStateImpl
  );

  $.Method({Static:true , Public:true }, "GetState", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.KeyboardState"), [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.PlayerIndex")], [])), 
    getStateImpl
  );
});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.KeyboardState", function ($) {
  $.RawMethod(false, "__CopyMembers__", function (source, target) {
    if (source.keys)
      target.keys = Array.prototype.slice.call(source.keys);
    else
      target.keys = [];
  });

  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [$jsilcore.TypeRef("System.Array", [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.Keys")])], [])), 
    function _ctor (keys) {
      this.keys = [];

      for (var i = 0; i < keys.length; i++)
        this.keys.push(keys[i].valueOf());
    }
  );

  $.Method({Static:false, Public:true }, "GetPressedKeys", 
    (new JSIL.MethodSignature($jsilcore.TypeRef("System.Array", [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.Keys")]), [], [])), 
    function GetPressedKeys () {
      if (!this.keys)
        return [];

      var result = [];
      var tKeys = $xnaasms[0].Microsoft.Xna.Framework.Input.Keys.__Type__;

      for (var i = 0, l = this.keys.length; i < l; i++)
        result.push(tKeys.$Cast(this.keys[i]));

      return result;
    }
  );

  $.Method({Static:false, Public:true }, "IsKeyDown", 
    (new JSIL.MethodSignature($.Boolean, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.Keys")], [])), 
    function IsKeyDown (key) {
      if (!this.keys)
        return false;

      return this.keys.indexOf(key.valueOf()) !== -1;
    }
  );

  $.Method({Static:false, Public:true }, "IsKeyUp", 
    (new JSIL.MethodSignature($.Boolean, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.Keys")], [])), 
    function IsKeyUp (key) {
      if (!this.keys)
        return true;

      return this.keys.indexOf(key.valueOf()) === -1;
    }
  );
});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.Mouse", function ($) {
  var getStateImpl = function (playerIndex) {
    var buttons = JSIL.Host.getHeldMouseButtons();
    var position = JSIL.Host.getMousePosition();

    var pressed = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Pressed;
    var released = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Released;

    // FIXME
    return new Microsoft.Xna.Framework.Input.MouseState(
      position[0], position[1], 0,
      (buttons.indexOf(0) >= 0) ? pressed : released,
      (buttons.indexOf(1) >= 0) ? pressed : released,
      (buttons.indexOf(2) >= 0) ? pressed : released,
      released, released
    );
  };

  $.Method({Static:true , Public:true }, "GetState", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.MouseState"), [], [])), 
    getStateImpl
  );
});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.MouseState", function ($) {

  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [
          $.Int32, $.Int32, 
          $.Int32, $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), 
          $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), 
          $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState")
        ], [])), 
    function _ctor (x, y, scrollWheel, leftButton, middleButton, rightButton, xButton1, xButton2) {
      // FIXME
      this.x = x;
      this.y = y;
      this.leftButton = leftButton;
      this.middleButton = middleButton;
      this.rightButton = rightButton;
    }
  );

  $.Method({Static:false, Public:true }, "get_LeftButton", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
    function get_LeftButton () {
      return this.leftButton;
    }
  );

  $.Method({Static:false, Public:true }, "get_MiddleButton", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
    function get_MiddleButton () {
      return this.middleButton;
    }
  );

  $.Method({Static:false, Public:true }, "get_RightButton", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
    function get_RightButton () {
      return this.rightButton;
    }
  );

  $.Method({Static:false, Public:true }, "get_X", 
    (new JSIL.MethodSignature($.Int32, [], [])), 
    function get_X () {
      return this.x;
    }
  );

  $.Method({Static:false, Public:true }, "get_Y", 
    (new JSIL.MethodSignature($.Int32, [], [])), 
    function get_Y () {
      return this.y;
    }
  );
});

$jsilxna.deadZone = function (value, max, deadZoneSize) {
  if (value < -deadZoneSize)
    value += deadZoneSize;
  else if (value <= deadZoneSize)
    return 0;
  else
    value -= deadZoneSize;

  var scaled = value / (max - deadZoneSize);
  if (scaled < -1)
    scaled = -1;
  else if (scaled > 1)
    scaled = 1;

  return scaled;
};

$jsilxna.deadZoneToPressed = function (value, max, deadZoneSize) {
  var pressed = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Pressed;
  var released = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Released;

  var scaled = $jsilxna.deadZone(value, max, deadZoneSize);
  if (Math.abs(scaled) > 0)
    return pressed;
  else
    return released;
};

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.GamePad", function ($) {
  var buttons = $xnaasms[0].Microsoft.Xna.Framework.Input.Buttons;
  var pressed = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Pressed;
  var released = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Released;

  var buttonsFromGamepadState = function (state) {
    var buttonStates = 0;

    if (state.faceButton0)
      buttonStates |= buttons.A;
    if (state.faceButton1)
      buttonStates |= buttons.B;
    if (state.faceButton2)
      buttonStates |= buttons.X;
    if (state.faceButton3)
      buttonStates |= buttons.Y;

    if (state.leftShoulder0)
      buttonStates |= buttons.LeftShoulder;
    if (state.rightShoulder0)
      buttonStates |= buttons.RightShoulder;

    if (state.select)
      buttonStates |= buttons.Back;
    if (state.start)
      buttonStates |= buttons.Start;

    if (state.leftStickButton)
      buttonStates |= buttons.LeftStick;
    if (state.rightStickButton)
      buttonStates |= buttons.RightStick;

    var result = buttons.$Cast(buttonStates);
    return result;
  };

  var getStateImpl = function (playerIndex) {
    var connected = false;
    var buttonStates = 0;
    var leftThumbstick = new Microsoft.Xna.Framework.Vector2(0, 0);
    var rightThumbstick = new Microsoft.Xna.Framework.Vector2(0, 0);
    var leftTrigger = 0, rightTrigger = 0;
    var dpadUp = released, dpadDown = released, dpadLeft = released, dpadRight = released;

    var svc = JSIL.Host.getService("gamepad", true);
    var state = null;
    if (svc)
      state = svc.getState(playerIndex.value);

    if (state) {
      connected = true;

      var blockInput = $jsilbrowserstate && $jsilbrowserstate.blockGamepadInput;

      if (!blockInput) {
        buttonStates = buttonsFromGamepadState(state);

        // FIXME: This is IndependentAxes mode. Maybe handle Circular too?
        var leftStickDeadZone = 7849 / 32767;
        var rightStickDeadZone = 8689 / 32767;

        leftThumbstick.X  = $jsilxna.deadZone(state.leftStickX, 1, leftStickDeadZone);
        rightThumbstick.X = $jsilxna.deadZone(state.rightStickX, 1, rightStickDeadZone);

        // gamepad.js returns inverted Y compared to XInput... weird.
        leftThumbstick.Y  = -$jsilxna.deadZone(state.leftStickY, 1, leftStickDeadZone);
        rightThumbstick.Y = -$jsilxna.deadZone(state.rightStickY, 1, rightStickDeadZone);

        leftTrigger  = state.leftShoulder1;
        rightTrigger = state.rightShoulder1;

        dpadUp    = state.dpadUp    ? pressed : released;
        dpadDown  = state.dpadDown  ? pressed : released;
        dpadLeft  = state.dpadLeft  ? pressed : released;
        dpadRight = state.dpadRight ? pressed : released;
      }
    }

    var buttons = new Microsoft.Xna.Framework.Input.GamePadButtons(
      buttonStates
    );

    var thumbs = new Microsoft.Xna.Framework.Input.GamePadThumbSticks(
      leftThumbstick, rightThumbstick
    );

    var triggers = new Microsoft.Xna.Framework.Input.GamePadTriggers(
      leftTrigger, rightTrigger
    );

    var dpad = new Microsoft.Xna.Framework.Input.GamePadDPad(
      dpadUp, dpadDown, dpadLeft, dpadRight
    );

    var result = JSIL.CreateInstanceOfType(
      Microsoft.Xna.Framework.Input.GamePadState.__Type__,
      "$internalCtor",
      [connected, thumbs, triggers, buttons, dpad]
    );
    return result;
  };

  $.Method({Static:true , Public:true }, "GetState", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadState"), [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.PlayerIndex")], [])), 
    getStateImpl
  );

  $.Method({Static:true , Public:true }, "GetState", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadState"), [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.PlayerIndex"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadDeadZone")], [])), 
    getStateImpl
  );

  $.Method({Static:true , Public:true }, "SetVibration", 
    (new JSIL.MethodSignature($.Boolean, [
          $xnaasms[0].TypeRef("Microsoft.Xna.Framework.PlayerIndex"), $.Single, 
          $.Single
        ], [])), 
    function SetVibration (playerIndex, leftMotor, rightMotor) {
      // FIXME
    }
  );

  $.Method({Static:true , Public:true }, "GetCapabilities", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadCapabilities"), [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.PlayerIndex")], [])), 
    function GetCapabilities (playerIndex) {
      var svc = JSIL.Host.getService("gamepad", true);
      var state = null;
      if (svc)
        state = svc.getState(playerIndex.value);

      var result = JSIL.CreateInstanceOfType(
        Microsoft.Xna.Framework.Input.GamePadCapabilities.__Type__,
        "$internalCtor",
        [Boolean(state)]
      );
      return result;
    }
  );
});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.GamePadCapabilities", function ($) {
  
  $.RawMethod(false, "$internalCtor", function (connected) {
    this._connected = connected;
  });

  $.Method({Static:false, Public:false}, ".ctor", 
    (new JSIL.MethodSignature(null, [$jsilcore.TypeRef("JSIL.Reference", [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.XINPUT_CAPABILITIES")]), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.ErrorCodes")], [])), 
    function _ctor (/* ref */ caps, result) {
      this._connected = false;
    }
  );

  $.Method({Static:false, Public:true }, "get_IsConnected", 
    (new JSIL.MethodSignature($.Boolean, [], [])), 
    function get_IsConnected () {
      return this._connected;
    }
  );

});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.GamePadState", function ($) {
  var buttons = $xnaasms[0].Microsoft.Xna.Framework.Input.Buttons;
  var pressed = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Pressed;
  var released = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Released;

  $.RawMethod(false, "$internalCtor", function GamePadState_internalCtor (connected, thumbSticks, triggers, buttons, dPad) {
    this._connected = connected;
    this._thumbs = thumbSticks;
    this._buttons = buttons;
    this._triggers = triggers;
    this._dpad = dPad;
  });

  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [
          $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadThumbSticks"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadTriggers"), 
          $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadButtons"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadDPad")
        ], [])), 
    function _ctor (thumbSticks, triggers, buttons, dPad) {
      this.$internalCtor(false, thumbSticks, triggers, buttons, dPad);
    }
  );

  $.Method({Static:false, Public:true }, "get_Buttons", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadButtons"), [], [])), 
    function get_Buttons () {
      return this._buttons;
    }
  );

  $.Method({Static:false, Public:true }, "get_DPad", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadDPad"), [], [])), 
    function get_DPad () {
      return this._dpad;
    }
  );

  $.Method({Static:false, Public:true }, "get_IsConnected", 
    (new JSIL.MethodSignature($.Boolean, [], [])), 
    function get_IsConnected () {
      // FIXME
      return this._connected;
    }
  );

  $.Method({Static:false, Public:true }, "get_ThumbSticks", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadThumbSticks"), [], [])), 
    function get_ThumbSticks () {
      return this._thumbs;
    }
  );

  $.Method({Static:false, Public:true }, "get_Triggers", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.GamePadTriggers"), [], [])), 
    function get_Triggers () {
      return this._triggers;
    }
  );

  var getButtonState = function (self, button) {
    var s = self._buttons._state;
    var key = button.valueOf();

    if (s && s[key])
      return s[key].valueOf();

    var triggerDeadZone = 30 / 255;

    switch (key) {
      // DPad
      case 1:
        return self._dpad._up;
      case 2:
        return self._dpad._down;
      case 4:
        return self._dpad._left;
      case 8:
        return self._dpad._right;

      // Triggers
      case 8388608:
        return $jsilxna.deadZoneToPressed(self._triggers._left, 1, triggerDeadZone);
      case 4194304:
        return $jsilxna.deadZoneToPressed(self._triggers._right, 1, triggerDeadZone);
    }

    return released;
  };

  $.Method({Static:false, Public:true }, "IsButtonDown", 
    (new JSIL.MethodSignature($.Boolean, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.Buttons")], [])), 
    function IsButtonDown (button) {
      return (getButtonState(this, button).valueOf() !== 0);
    }
  );

  $.Method({Static:false, Public:true }, "IsButtonUp", 
    (new JSIL.MethodSignature($.Boolean, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.Buttons")], [])), 
    function IsButtonUp (button) {
      return (getButtonState(this, button).valueOf() === 0);
    }
  );

});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.GamePadButtons", function ($) {
  var buttonNames = [
    "A", "B", "Back", "BigButton", 
    "LeftShoulder", "LeftStick", "RightShoulder", "RightStick",
    "Start", "X", "Y"
  ];

  var buttons = $xnaasms[0].Microsoft.Xna.Framework.Input.Buttons;
  var pressed = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Pressed;
  var released = $xnaasms[0].Microsoft.Xna.Framework.Input.ButtonState.Released;

  var makeButtonGetter = function (buttonName) {
    return function getButtonState () {
      if (!this._state)
        return released;
      
      var key = buttons[buttonName].valueOf();
      return this._state[key] || released;
    };
  }

  for (var i = 0; i < buttonNames.length; i++) {
    var buttonName = buttonNames[i];

    $.Method({Static:false, Public:true }, "get_" + buttonName, 
      (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
      makeButtonGetter(buttonName)
    );
  }

  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.Buttons")], [])), 
    function _ctor (buttonState) {
      this._state = {};

      buttonState = buttonState.valueOf();

      for (var i = 0; i < buttonNames.length; i++) {
        var buttonName = buttonNames[i];
        var buttonMask = buttons[buttonName].valueOf();

        this._state[buttonMask] = (buttonState & buttonMask) ? pressed : released;
      }
    }
  );

  $.RawMethod(false, "__CopyMembers__", 
    function GamePadButtons_CopyMembers (source, target) {
      target._state = {};

      for (var k in source._state) {
        if (!source._state.hasOwnProperty(k))
          continue;

        target._state[k] = source._state[k];
      }
    }
  );

});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.GamePadThumbSticks", function ($) {
  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Vector2"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Vector2")], [])), 
    function _ctor (leftThumbstick, rightThumbstick) {
      this._left = leftThumbstick;
      this._right = rightThumbstick;
    }
  );

  $.Method({Static:false, Public:true }, "get_Left", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Vector2"), [], [])), 
    function get_Left () {
      return this._left;
    }
  );

  $.Method({Static:false, Public:true }, "get_Right", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Vector2"), [], [])), 
    function get_Right () {
      return this._right;
    }
  );
});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.GamePadDPad", function ($) {

  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [
          $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), 
          $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), $xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState")
        ], [])), 
    function _ctor (upValue, downValue, leftValue, rightValue) {
      this._up = upValue;
      this._down = downValue;
      this._left = leftValue;
      this._right = rightValue;
    }
  );

  $.Method({Static:false, Public:true }, "get_Down", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
    function get_Down () {
      return this._down;
    }
  );

  $.Method({Static:false, Public:true }, "get_Left", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
    function get_Left () {
      return this._left;
    }
  );

  $.Method({Static:false, Public:true }, "get_Right", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
    function get_Right () {
      return this._right;
    }
  );

  $.Method({Static:false, Public:true }, "get_Up", 
    (new JSIL.MethodSignature($xnaasms[0].TypeRef("Microsoft.Xna.Framework.Input.ButtonState"), [], [])), 
    function get_Up () {
      return this._up;
    }
  );

});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.GamePadTriggers", function ($) {
  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [$.Single, $.Single], [])), 
    function _ctor (leftTrigger, rightTrigger) {
      this._left = leftTrigger;
      this._right = rightTrigger;
    }
  );

  $.Method({Static:false, Public:true }, "get_Left", 
    (new JSIL.MethodSignature($.Single, [], [])), 
    function get_Left () {
      return this._left;
    }
  );

  $.Method({Static:false, Public:true }, "get_Right", 
    (new JSIL.MethodSignature($.Single, [], [])), 
    function get_Right () {
      return this._right;
    }
  );

});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.Touch.TouchPanel", function ($) {
  $.Method({Static:true , Public:true }, "GetState", 
    (new JSIL.MethodSignature($xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchCollection"), [], [])), 
    function GetState () {
      return new Microsoft.Xna.Framework.Input.Touch.TouchCollection(null);
    }
  );

  $.Method({Static:true , Public:true }, "ReadGesture", 
    (new JSIL.MethodSignature($xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.GestureSample"), [], [])), 
    function ReadGesture () {
      throw new Error('Not implemented');
    }
  );
});

JSIL.ImplementExternals("Microsoft.Xna.Framework.Input.Touch.TouchCollection", function ($) {
  $.Method({Static:false, Public:true }, ".ctor", 
    (new JSIL.MethodSignature(null, [$jsilcore.TypeRef("System.Array", [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")])], [])), 
    function _ctor (touches) {
      // FIXME
      this.touches = touches;
    }
  );

  $.Method({Static:false, Public:true }, "Add", 
    (new JSIL.MethodSignature(null, [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")], [])), 
    function Add (item) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "Clear", 
    (new JSIL.MethodSignature(null, [], [])), 
    function Clear () {
      // FIXME
    }
  );

  $.Method({Static:false, Public:true }, "Contains", 
    (new JSIL.MethodSignature($.Boolean, [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")], [])), 
    function Contains (item) {
      // FIXME
      return false;
    }
  );

  $.Method({Static:false, Public:true }, "CopyTo", 
    (new JSIL.MethodSignature(null, [$jsilcore.TypeRef("System.Array", [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")]), $.Int32], [])), 
    function CopyTo (array, arrayIndex) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "FindById", 
    (new JSIL.MethodSignature($.Boolean, [$.Int32, $jsilcore.TypeRef("JSIL.Reference", [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")])], [])), 
    function FindById (id, /* ref */ touchLocation) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "get_Count", 
    (new JSIL.MethodSignature($.Int32, [], [])), 
    function get_Count () {
      // FIXME
      return 0;
    }
  );

  $.Method({Static:false, Public:true }, "get_IsConnected", 
    (new JSIL.MethodSignature($.Boolean, [], [])), 
    function get_IsConnected () {
      // FIXME
      return false;
    }
  );

  $.Method({Static:false, Public:true }, "get_IsReadOnly", 
    (new JSIL.MethodSignature($.Boolean, [], [])), 
    function get_IsReadOnly () {
      // FIXME
      return true;
    }
  );

  $.Method({Static:false, Public:true }, "get_Item", 
    (new JSIL.MethodSignature($xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation"), [$.Int32], [])), 
    function get_Item (index) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "GetEnumerator", 
    (new JSIL.MethodSignature($xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchCollection/Enumerator"), [], [])), 
    function GetEnumerator () {
      // Stupid XNA samples calling this without checking IsConnected, grrr
      // FIXME
      return JSIL.GetEnumerator([]);
    }
  );

  $.Method({Static:true , Public:false}, "GetLocInfo", 
    (new JSIL.MethodSignature($.Boolean, [
          $jsilcore.TypeRef("JSIL.Reference", [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.XNAINPUT_TOUCH_LOCATION_STATE")]), $.Int32, 
          $jsilcore.TypeRef("JSIL.Reference", [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchCollection/LocInfo")])
        ], [])), 
    function GetLocInfo (/* ref */ state, index, /* ref */ locInfo) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:false}, "GetPreviousLocation", 
    (new JSIL.MethodSignature($.Boolean, [
          $.Int32, $.Int32, 
          $jsilcore.TypeRef("JSIL.Reference", [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")])
        ], [])), 
    function GetPreviousLocation (id, prevLocationCount, /* ref */ location) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "IndexOf", 
    (new JSIL.MethodSignature($.Int32, [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")], [])), 
    function IndexOf (item) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "Insert", 
    (new JSIL.MethodSignature(null, [$.Int32, $xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")], [])), 
    function Insert (index, item) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "Remove", 
    (new JSIL.MethodSignature($.Boolean, [$xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")], [])), 
    function Remove (item) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "RemoveAt", 
    (new JSIL.MethodSignature(null, [$.Int32], [])), 
    function RemoveAt (index) {
      throw new Error('Not implemented');
    }
  );

  $.Method({Static:false, Public:true }, "set_Item", 
    (new JSIL.MethodSignature(null, [$.Int32, $xnaasms[4].TypeRef("Microsoft.Xna.Framework.Input.Touch.TouchLocation")], [])), 
    function set_Item (index, value) {
      throw new Error('Not implemented');
    }
  );
});
