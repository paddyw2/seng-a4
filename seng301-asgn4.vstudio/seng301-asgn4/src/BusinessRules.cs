using System;
using System.Collections.Generic;
using Frontend4;
using Frontend4.Hardware;

// keeps track of product prices, how
// excess change is dealt with etc.
// acts as controller class - directs
// the funds, commication and product
// facades
public class BusinessRules
{
    private CommunicationFacade comms;
    private ProductFacade product;
    private PaymentFacade payment;

    private int insertedAmount;

    public BusinessRules(CommunicationFacade comms, ProductFacade product, PaymentFacade payment)
    {
        this.comms = comms;
        this.product = product;
        this.payment = payment;
        insertedAmount = 0;

        payment.MoneyInserted += new EventHandler<MoneyEventArgs>(incrementInsertValue);
        comms.ButtonPressed += new EventHandler<ButtonIdEventArgs>(buttonPressed);

    }

    public void Configure(List<ProductKind> products)
    {
        product.Configure(products);
    }

    public void processButtonSelection(int id)
    {
        // MAIN LOGIC
        // takes a button id as argument, and
        // if the selection is valid, then dispenses
        // appropriate product and change
        int price = product.getPrice(id);
        int productQuantity = product.getQuantity(id);
        // check if enough money inserted
        if (insertedAmount >= price && productQuantity > 0)
        {
            // dispense pop
            product.dispenseProduct(id);
            payment.acceptCoins();
            // calculate and dispense change
            int change = insertedAmount - price;
            int remainingCredit = payment.dispenseChange(change);
            // reset inserted amount taking into
            // account any credit
            insertedAmount = remainingCredit;
            Console.WriteLine("Remaining credit: " + remainingCredit);
        }
        else
        {
            if (productQuantity > 0)
                Console.WriteLine("Not enough money entered");
            else
                Console.WriteLine("Not enough pop");
        }
    }

    // EVENT LISTENERS

    public void incrementInsertValue(object sender, MoneyEventArgs e)
    {
        insertedAmount = insertedAmount + e.value;
    }

    public void buttonPressed(object sender, ButtonIdEventArgs e)
    {
        processButtonSelection(e.buttonId);
    }

}