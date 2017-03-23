// Class: ButtonIdEventArgs
// Provides a custom event
// object that contains an
// int value that represents
// the id of a pressed button

using System;
using System.Collections.Generic;

namespace Frontend4.Hardware {

    public class ButtonIdEventArgs : EventArgs {
        public int buttonId { get; set; }
    }
}