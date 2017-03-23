// Class: BusinessRules
// Authorizes and processes
// vending machine purchases
// by communicating with and
// managing sub facades

using System;
using System.Collections.Generic;
using Frontend4;
using Frontend4.Hardware;

public class BusinessRules
{
    private CommunicationFacade comms;
    private ProductFacade product;
    private PaymentFacade payment;
    private int insertedAmount;

    public BusinessRules(CommunicationFacade comms, ProductFacade product, PaymentFacade payment)
    {
        // save sub facade instances
        this.comms = comms;
        this.product = product;
        this.payment = payment;
        insertedAmount = 0;

        // subscribe to sub facade events
        payment.MoneyInserted += new EventHandler<MoneyEventArgs>(incrementInsertValue);
        comms.ButtonPressed += new EventHandler<ButtonIdEventArgs>(buttonPressed);
    }

    public void Configure(List<ProductKind> products)
    {
        product.Configure(products);
    }

    public void processButtonSelection(int id)
    {
        // MAIN LOGIC //
        // takes a button id as argument, and
        // if the selection is valid, then dispenses
        // appropriate product and change

        // get selected product price
        int price = product.getPrice(id);
        // get selected product quantity
        int productQuantity = product.getQuantity(id);
        // check if enough money inserted for purchase
        if (insertedAmount >= price && productQuantity > 0)
        {
            // if selection valid, dispense pop
            product.dispenseProduct(id);
            // takes payment into machine
            payment.acceptPayment(price);
            // calculate and dispense change if required
            int change = insertedAmount - price;
            int remainingCredit = payment.dispenseChange(change);
            // reset inserted amount, taking into
            // account any credit
            insertedAmount = remainingCredit;
            comms.displayMessage("Remaining credit: " + remainingCredit);
        }
        else
        {
            if (productQuantity > 0)
                comms.displayMessage("Not enough money entered");
            else
                comms.displayMessage("Not enough products");
        }
    }

    // EVENT LISTENERS //

    public void incrementInsertValue(object sender, MoneyEventArgs e)
    {
        insertedAmount = insertedAmount + e.value;
    }

    public void buttonPressed(object sender, ButtonIdEventArgs e)
    {
        processButtonSelection(e.buttonId);
    }
}