using System;
using System.Collections.Generic;
using Frontend4;
using Frontend4.Hardware;
// must subscribe to events, then deposit products
// also in charge of refilling products
public class ProductFacade
{
    private HardwareFacade facade;
    private List<ProductKind> productSpecs;

    public ProductFacade(HardwareFacade facade)
    {
        this.facade = facade;

    }

    public void Configure(List<ProductKind> products)
    {
        productSpecs = products;
        facade.Configure(products);
    }

    public int getPrice(int id)
    {
        return productSpecs[id].Cost.Value;
    }

    public int getQuantity(int id)
    {
        ProductRack[] racks = facade.ProductRacks;
        return racks[id].Count;
    }

    public void loadProducts(int id, List<Product> products)
    {
        foreach(var product in products)
        {
            // load specific product by button id
        }
    }

    public void dispenseProduct(int id)
    {
        ProductRack[] racks = facade.ProductRacks;
        racks[id].DispenseProduct();
    }

    // returns specificed product cost by name
    public int getProductCost(string productName)
    {
        foreach (var product in productSpecs)
        {
            if (product.Name.Equals(productName))
                return product.Cost.Value;
        }

        return -1;
    }
}