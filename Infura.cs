using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AllNFTs : MonoBehaviour
{
    // Feel free to change all of this. It's purely educational and nothing more than a proof of concept
    public string APIKey;
    public string APICall;


    [Serializable]
    public class TokenData
    {
        public Owners[] owners;
    }

    [Serializable]
    public class Owners
    {
        public string ownerOf { get; set; }
        public string tokenAddress { get; set; }

        // You can add more key:value pairs here
    }


    public void Start()
    {
        // Add your Infura API Key and Secret in the format shown below
        APIKey = "<API KEY>:<API SECRET>";

        // https://docs.infura.io/infura/infura-expansion-apis/nft-api/rest-apis/api-reference
        // The below URL will call all the owners of a specific NFT on a specified network and contract.
        APICall = "https://nft.api.infura.io/networks/338/nfts/0x766D1AB3Badf9a5E3cc71dA0408B7125a16e2D45/2/owners";

        // Invoke the coroutine
        StartCoroutine(CallInfura(APICall));
    }

    IEnumerator CallInfura(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            var APIAuth = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(APIKey));

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", APIAuth);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);

                    // Deserialize our JSON object
                    TokenData tokens = JsonConvert.DeserializeObject<TokenData>(webRequest.downloadHandler.text);

                    // Iterate through all the owners within the Owner class
                    foreach (Owners owner in tokens.owners)
                    {
                        // Debug the wallets that own this
                        Debug.Log("Owner Wallet: " + owner.ownerOf);

                        // Code can be executed here to validate if the connected wallet is one of the listed wallets.
                    }
                    break;
            }
        }
    }
}
