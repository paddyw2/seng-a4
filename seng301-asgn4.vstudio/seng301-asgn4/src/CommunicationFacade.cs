using System;
using System.Collections.Generic;
using Frontend4;
using Frontend4.Hardware;

// this is where the display type is defined (?),
// allows for easy configuration of displays
// listens for events, then directs them to
// the configured display

public class CommunicationFacade
{
    private HardwareFacade facade;
    public event EventHandler<ButtonIdEventArgs> ButtonPressed;

    public CommunicationFacade(HardwareFacade facade)
    {
        this.facade = facade;
        SelectionButton[] buttons = facade.SelectionButtons;
        // add handler to each button
        foreach (SelectionButton button in buttons)
        {
            button.Pressed += new EventHandler(printButtonPressed);
        }

        CoinSlot coinSlot = facade.CoinSlot;
        coinSlot.CoinRejected += new EventHandler<CoinEventArgs>(printCoinRejected);
        coinSlot.CoinAccepted += new EventHandler<CoinEventArgs>(printCoinAccepted);

        CoinRack[] coinRacks = facade.CoinRacks;
        // add handler to each coin rack
        foreach (CoinRack coinRack in coinRacks)
        {
            coinRack.CoinAdded += new EventHandler<CoinEventArgs>(printCoinLoaded);
        }
        ProductRack[] productRacks = facade.ProductRacks;
        // add handler to each pop rack
        foreach (ProductRack rack in productRacks)
        {
            rack.ProductRemoved += new EventHandler<ProductEventArgs>(printPopDispensed);
        }
    }

    public void buttonPressed(int val)
    {
        this.ButtonPressed(this, new ButtonIdEventArgs() { buttonId = val });
    }

    public void displayMessage(string message)
    {
        facade.Display.DisplayMessage(message);
    }

    // DISPLAY METHODS //
    public void printCoinRejected(Object sender, CoinEventArgs e)
    {
        Console.WriteLine("Coin rejected");
        displayMessage("Coin rejected");
    }

    public void printCoinAccepted(Object sender, CoinEventArgs e)
    {
        Console.WriteLine("Coin accepted");
        displayMessage("Coin accepted");
    }

    public void printButtonPressed(Object sender, EventArgs e)
    {
        Console.WriteLine("Button thingy");
        SelectionButton b = (SelectionButton)sender;
        SelectionButton[] buttons = facade.SelectionButtons;
        int id = 0;
        foreach(var button in buttons)
        {
            if (b == button)
                break;
            id++;
        }
        if (id > buttons.Length)
            Console.WriteLine("No match found! ####");

        this.ButtonPressed(this, new ButtonIdEventArgs() { buttonId = id });
        Console.WriteLine("Button pressed");
        displayMessage("Button pressed");
    }

    public void printCoinLoaded(Object sender, CoinEventArgs e)
    {
        Console.WriteLine("Coin loaded");
        displayMessage("Coin loaded");
    }

    public void printPopDispensed(Object sender, ProductEventArgs e)
    {
        Console.WriteLine("Product dispensed");
        displayMessage("Product dispensed");
    }

}