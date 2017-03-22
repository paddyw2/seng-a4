using System;
using System.Collections.Generic;
using Frontend4;
using Frontend4.Hardware;

// in theory, handles different
// types of payment - presumably
// they use a card, whatever they
// pay is converted to coins
public class PaymentFacade
{
    private HardwareFacade facade;
    private int[] coinTypes;
    public event EventHandler<MoneyEventArgs> MoneyInserted;

    public PaymentFacade(HardwareFacade facade)
    {
        this.facade = facade;
        CoinSlot coinSlot = facade.CoinSlot;
        coinSlot.CoinAccepted += new EventHandler<CoinEventArgs>(coinAccepted);
    }

    public void setCoinTypes(Cents[] types)
    {
        coinTypes = new int[types.Length];
        for(int i=0;i<types.Length;i++)
        {
            coinTypes[i] = types[i].Value;
        }
    }

    public void acceptCoins()
    {
        facade.CoinReceptacle.StoreCoins();
    }

    public int dispenseChange(int change)
    {
        /* Change Algorithm */
        // change algorithm from A1 (slightly modified)
        int val = change;
        int upperBound = val + 1;

        int largestCoinVal = 0;
        // loop change process until either
        // no more valid coins, or change
        // value is met (where the loop will
        // be broken)
        while (true)
        {
            // loop through coin slots to find
            // next denomination
            // if found, sets the largestSlot
            // to a Coin, which has a value
            CoinRack largestSlot = null;
            CoinRack[] coinRacks = facade.CoinRacks;
            int slotIndex = 0;
            int counter = 0;
            foreach (CoinRack slot in coinRacks)
            {
                int rackValue = coinTypes[counter];
                if (rackValue >= largestCoinVal && rackValue < upperBound)
                {
                    largestCoinVal = rackValue;
                    largestSlot = slot;
                    slotIndex = counter;
                }
                counter++;
            }

            // check if largest coin is null
            // if so, then we have not found
            // a suitable change coin (we will
            // short change them)
            if (largestSlot == null)
                break;

            // now we have next largest coin
            // decrease until target met, each
            // time removing a coin from the
            // coin slot, and adding it to the
            // coin change list
            bool runLoop = true;
            bool changeFinished = false;
            while (runLoop && largestSlot.Count > 0)
            {
                // decrement change total by coin
                // denomination
                val = val - largestCoinVal;
                if (val >= 0)
                {
                    // returned coin may be "incorrect" if the
                    // slot was loaded incorrectly - this is
                    // the desired functionality
                    CoinRack[] racks = facade.CoinRacks;
                    // release coin in hardware and in software
                    coinRacks[slotIndex].ReleaseCoin();
                    Console.WriteLine("Dispensing coin: " + largestCoinVal);

                    // if new change value is zero, all change
                    // has been added to the coin change list
                    // so terminate loop
                    if (val == 0)
                    {
                        runLoop = false;
                        changeFinished = true;
                    }
                }
                else
                {
                    // if change value is negative, reverse
                    // last decrement and move to lower
                    // denomination
                    val = val + largestCoinVal;
                    runLoop = false;
                }
            }

            // if change finished flag is set, then
            // exit main loop
            if (changeFinished)
                break;

            // else, reset variables and start next
            // loop to find lower denomination
            upperBound = largestCoinVal;
            largestCoinVal = 0;
        }
        return val;
    }

    // triggers an event indicating how much "money" inserted
    // rather than coins
    public void insertedPayment(int val)
    {
        this.MoneyInserted(this, new MoneyEventArgs() { value = val });
    }

    // captures coin inserted event, converts it to "value inserted"
    // and triggers generic event
    public void coinAccepted(object sender, CoinEventArgs e)
    {
        insertedPayment(e.Coin.Value.Value);
    }

}