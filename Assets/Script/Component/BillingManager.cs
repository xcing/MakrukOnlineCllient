﻿////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using Assets.Script.chess_thai;
namespace Assets.Script.Component
{
    public class BillingManager : MonoBehaviour
    {

        private static bool _isInited = false;

        //--------------------------------------
        //  INITIALIZE
        //--------------------------------------


        //replace with your consumable item
        //public const string COINS_ITEM = "small_coins_bag";

        //replace with your non-consumable item
        //public const string COINS_BOOST = "coins_bonus";

        public const string NO_ADS_PRODUCT_ID = "no_ads";
        public const string ONE_DOLLAR_PRODUCT_ID = "one";
        public const string FIVE_DOLLAR_PRODUCT_ID = "five";
        public const string TEN_DOLLAR_PRODUCT_ID = "ten";



        private static bool ListnersAdded = false;
        public static void init()
        {
            if (ListnersAdded)
            {
                return;
            }

            //Filling product list
            //You can skip this if you alredy did this in Editor settings menu
            AndroidInAppPurchaseManager.Instance.AddProduct(NO_ADS_PRODUCT_ID);
            AndroidInAppPurchaseManager.Instance.AddProduct(ONE_DOLLAR_PRODUCT_ID);
            AndroidInAppPurchaseManager.Instance.AddProduct(FIVE_DOLLAR_PRODUCT_ID);
            AndroidInAppPurchaseManager.Instance.AddProduct(TEN_DOLLAR_PRODUCT_ID);


            //listening for purchase and consume events

            AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
            AndroidInAppPurchaseManager.ActionProductConsumed += OnProductConsumed;
            AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;

            //you may use loadStore function without parametr if you have filled base64EncodedPublicKey in plugin settings
            AndroidInAppPurchaseManager.Instance.LoadStore();

            ListnersAdded = true;


        }

        //--------------------------------------
        //  PUBLIC METHODS
        //--------------------------------------


        public static void purchase(string SKU)
        {
            AndroidInAppPurchaseManager.Instance.Purchase(SKU);
        }

        public static void consume(string SKU)
        {
            AndroidInAppPurchaseManager.Instance.Consume(SKU);
        }

        //--------------------------------------
        //  GET / SET
        //--------------------------------------

        public static bool isInited
        {
            get
            {
                return _isInited;
            }
        }


        //--------------------------------------
        //  EVENTS
        //--------------------------------------

        private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase)
        {
            //some stuff for processing product purchse. Add coins, unlock track, etc

            switch (purchase.SKU)
            {
                case ONE_DOLLAR_PRODUCT_ID:
                    GameObject.Find("ThemeScript").GetComponent<Theme>().BuyCoin(1000);
                    break;
                case FIVE_DOLLAR_PRODUCT_ID:
                    GameObject.Find("ThemeScript").GetComponent<Theme>().BuyCoin(7000);
                    break;
                case TEN_DOLLAR_PRODUCT_ID:
                    GameObject.Find("ThemeScript").GetComponent<Theme>().BuyCoin(18000);
                    break;
                case NO_ADS_PRODUCT_ID:
                    GameObject.Find("SelectModeScript").GetComponent<SelectMode>().BuyNoAds();
                    break;
            }
        }

        private static void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase)
        {
            switch (purchase.SKU)
            {
                /*case COINS_ITEM:
                    GameDataExample.AddCoins(100);
                    break;*/
            }
        }

        private static void OnProductPurchased(BillingResult result)
        {

            //this flag will tell you if purchase is available
            //result.isSuccess


            //infomation about purchase stored here
            //result.purchase

            //here is how for example you can get product SKU
            //result.purchase.SKU


            if (result.isSuccess)
            {
                OnProcessingPurchasedProduct(result.purchase);
            }
            else
            {
                //AndroidMessage.Create("Product Purchase Failed", result.response.ToString() + " " + result.message);
            }

            Debug.Log("Purchased Responce: " + result.response.ToString() + " " + result.message);
        }


        private static void OnProductConsumed(BillingResult result)
        {

            if (result.isSuccess)
            {
                OnProcessingConsumeProduct(result.purchase);
            }
            else
            {
                AndroidMessage.Create("Product Cousume Failed", result.response.ToString() + " " + result.message);
            }

            Debug.Log("Cousume Responce: " + result.response.ToString() + " " + result.message);
        }


        private static void OnBillingConnected(BillingResult result)
        {

            AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;

            if (result.isSuccess)
            {
                //Store connection is Successful. Next we loading product and customer purchasing details
                AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
                AndroidInAppPurchaseManager.Instance.RetrieveProducDetails();

            }

            //AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
            //Debug.Log("Connection Responce: " + result.response.ToString() + " " + result.message);
        }




        private static void OnRetrieveProductsFinised(BillingResult result)
        {
            AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
            if (result.isSuccess)
            {
                UpdateStoreData();
                _isInited = true;
            }
            else
            {
                //AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
            }
        }



        private static void UpdateStoreData()
        {

            foreach (GoogleProductTemplate p in AndroidInAppPurchaseManager.Instance.Inventory.Products)
            {
                Debug.Log("Loaded product: " + p.Title);
            }

            //chisking if we already own some consuamble product but forget to consume those
            if (AndroidInAppPurchaseManager.Instance.Inventory.IsProductPurchased(NO_ADS_PRODUCT_ID))
            {
                consume(NO_ADS_PRODUCT_ID);
            }

            //Check if non-consumable rpduct was purchased, but we do not have local data for it.
            //It can heppens if game was reinstalled or download on oher device
            //This is replacment for restore purchase fnunctionality on IOS


            if (AndroidInAppPurchaseManager.Instance.Inventory.IsProductPurchased(NO_ADS_PRODUCT_ID))
            {
                GameDataExample.EnableCoinsBoost();
            }


        }

    }
}
