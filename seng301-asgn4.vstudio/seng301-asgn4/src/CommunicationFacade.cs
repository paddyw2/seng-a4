// Class: Communication Facade
// Deals with the display of messages
// to the configured display from the
// hardware, and triggers generic
// events with custom information
// processed from raw hardware events

using System;
using System.Collections.Generic;
using Frontend4;
using Frontend4.Hardware;

public class CommunicationFacade
{
    private HardwareFacade facade;
    public event EventHandler<ButtonIdEventArgs> ButtonPressed;

    public CommunicationFacade(HardwareFacade facade)
    {
        // save hardware facade
        this.facade = facade;

        // Event Subscriptions //
        // selection buttons
        SelectionButton[] buttons = facade.SelectionButtons;
        foreach (SelectionButton button in buttons)
        {
            button.Pressed += new EventHandler(printButtonPressed);
        }
        // coin slots
        CoinSlot coinSlot = facade.CoinSlot;
        coinSlot.CoinRejected += new EventHandler<CoinEventArgs>(printCoinRejected);
        coinSlot.CoinAccepted += new EventHandler<CoinEventArgs>(printCoinAccepted);
        // coin racks
        CoinRack[] coinRacks = facade.CoinRacks;
        foreach (CoinRack coinRack in coinRacks)
        {
            coinRack.CoinAdded += new EventHandler<CoinEventArgs>(printCoinLoaded);
        }
        // product racks
        ProductRack[] productRacks = facade.ProductRacks;
        foreach (ProductRack rack in productRacks)
        {
            rack.ProductRemoved += new EventHandler<ProductEventArgs>(printPopDispensed);
        }
    }
    
    // displays provided message to the configured
    // display
    public void displayMessage(string message)
    {
        facade.Display.DisplayMessage(message);
    }


    // Display Events //

    public void printCoinRejected(Object sender, CoinEventArgs e)
    {
        displayMessage("Coin rejected");
    }

    public void printCoinAccepted(Object sender, CoinEventArgs e)
    {
        displayMessage("Coin accepted");
    }

    public void printButtonPressed(Object sender, EventArgs e)
    {
        SelectionButton b = (SelectionButton)sender;
        SelectionButton[] buttons = facade.SelectionButtons;

        // get button id by object reference
        int id = 0;
        foreach(var button in buttons)
        {
            if (b == button)
                break;
            id++;
        }

        // trigger ButtonPressed event with button id
        this.ButtonPressed(this, new ButtonIdEventArgs() { buttonId = id });
        displayMessage("Button pressed");
    }

    public void printCoinLoaded(Object sender, CoinEventArgs e)
    {
        displayMessage("Coin loaded");
    }

    public void printPopDispensed(Object sender, ProductEventArgs e)
    {
        displayMessage("Product dispensed");
    }
}