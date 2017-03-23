// Class: MoneyEventArgs
// Provides a custom event
// object that contains an
// int value that represents
// added monetary credit

using System;
using System.Collections.Generic;

namespace Frontend4.Hardware {

    public class MoneyEventArgs : EventArgs {
        public int value { get; set; }
    }
}