using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public GameObject warningPanel;
    public Text warningMessage;

    public static string SKU_PREMIUM = "premium";
    
    void Start()
    {
        if(ParameterReader.Instance.IsPremium)
            gameObject.SetActive(false);
        
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(SKU_PREMIUM, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyPremium()
    {
        Audio.Instance.PlaySound(Sound.Button);

        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(SKU_PREMIUM);

            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                warningPanel.SetActive(true);
                warningMessage.text = LocalizedText.Translate("premium_error", null);
            }
        }
        else
        {
            warningPanel.SetActive(true);
            warningMessage.text = LocalizedText.Translate("premium_error", null);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        Product product = m_StoreController.products.WithID(SKU_PREMIUM);
        if (product != null && product.hasReceipt)
        {
            PlayerPrefs.SetInt("IsPremium", 1);
            gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("IsPremium", 0);
        }
    }


    public void OnInitializeFailed(InitializationFailureReason error){}


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (string.Equals(args.purchasedProduct.definition.id, SKU_PREMIUM, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("IsPremium", 1);
        }
        else
        {
            warningPanel.SetActive(true);
            warningMessage.text = LocalizedText.Translate("premium_error", null);
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
    }

    public void HideWarning()
    {
        Audio.Instance.PlaySound(Sound.Button);
        warningPanel.gameObject.SetActive(false);
    }
}
