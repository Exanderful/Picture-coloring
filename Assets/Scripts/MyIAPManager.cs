using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.Purchasing.Security;
//using TMPro;


public class MyIAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private static UnityEngine.Purchasing.Product test_product = null;
    private IAppleExtensions m_AppleExtensions;
    private IGooglePlayStoreExtensions m_GoogleExtensions;

    public static string REMOVE_ADS = "remove_ads";

    public Button btnNoAds;

    private Boolean return_complete = true;

    private ConfigurationBuilder builder;

    void Awake()
    {

    }

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }
        
        builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable);

        //builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);

        UnityPurchasing.Initialize(this, builder);

        //ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();

        //foreach (ProductCatalogItem product in catalog.allProducts)
        // {
        //MyDebug("Product = " + product.id);
        //}
    }

    /*void OnDeferredPurchase(Product product)
    {
        MyDebug($"Purchase of {product.definition.id} is deferred");
        btnGold.enabled = false;

    }*/

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyNonConsumable()
    {
        BuyProductID(REMOVE_ADS);
    }

    public void CompletePurchase()
    {
        if (test_product == null)
            Debug.Log("Cannot complete purchase, product not initialized.");
        else
        {
            m_StoreController.ConfirmPendingPurchase(test_product);

            Debug.Log("Completed purchase with " + test_product.definition.id);
        }
    }

    /*public void FetchProducts()
    {
        var myHashSet = new HashSet<ProductDefinition>();

        ProductDefinition item = new ProductDefinition(GOLD_50, ProductType.Consumable);

       myHashSet.Add(item);

       m_StoreController.FetchAdditionalProducts(myHashSet, OnProductsFetched, OnInitializeFailed);
    }*/

    public void ToggleComplete()
    {
        return_complete = !return_complete;
        Debug.Log("Complete = " + return_complete);

    }

    /*public void OnProductsFetched()
    {
        MyDebug("In fetch...");
    }*/

    /*public void RestorePurchases()
    {
        //m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
        //    if (result)
        //    {
        //        MyDebug("Restore purchases succeeded.");
        //    }
        //    else
        //    {
        //        MyDebug("Restore purchases failed.");
        //    }
        //});
    }*/

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            UnityEngine.Purchasing.Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log("Purchasing product:" + product.definition.id);
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void ListProducts()
    {
        foreach (UnityEngine.Purchasing.Product item in m_StoreController.products.all)
        {
            if (item.receipt != null)
            {
                Debug.Log("Receipt found for Product = " + item.definition.id);
            }
        }
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_GoogleExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        Dictionary<string, string> dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

        foreach (UnityEngine.Purchasing.Product item in controller.products.all)
        {
            if (item.receipt != null)
            {
                string intro_json = (dict == null || !dict.ContainsKey(item.definition.storeSpecificId)) ? null : dict[item.definition.storeSpecificId];

                /*if (item.definition.type == ProductType.Subscription)
                {
                    SubscriptionManager p = new SubscriptionManager(item, null);
                    SubscriptionInfo info = p.getSubscriptionInfo();
                    MyDebug("SubInfo: " + info.getProductId().ToString());
                    MyDebug("getExpireDate: " + info.getExpireDate().ToString());
                    MyDebug("isSubscribed: " + info.isSubscribed().ToString());
                }*/
            }
        }
    }

    /*public void OnPurchaseDeferred(Product product)
    {

        MyDebug("Deferred product " + product.definition.id.ToString());
    }*/

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        test_product = args.purchasedProduct;

        Debug.Log("Receipt:" + test_product.receipt);

        if (return_complete)
        {
            Debug.Log("ProcessPurchase: Complete. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID);
            GetComponent<StoreManager>().OnRemoveAdsPurchaseComplete();
            return PurchaseProcessingResult.Complete;
        }
        else
        {
            Debug.Log("ProcessPurchase: Pending. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID);
            return PurchaseProcessingResult.Pending;
        }

     }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}" + " - " + product.definition.storeSpecificId + " - " + failureReason);
    }
}
