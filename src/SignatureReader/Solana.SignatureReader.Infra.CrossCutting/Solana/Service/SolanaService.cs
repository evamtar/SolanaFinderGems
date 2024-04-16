using Newtonsoft.Json;
using Solana.SignatureReader.Domain.Model.CrossCutting.Solana.Request;
using Solana.SignatureReader.Domain.Model.CrossCutting.Solana.Response;
using Solana.SignatureReader.Domain.Service.CrossCutting.Solana;
using System.Text;

namespace Solana.SignatureReader.Infra.CrossCutting.Solana.Service
{
    public class SolanaService : ISolanaService
    {
        private readonly HttpClient _httpClient;


        public SolanaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://solana-mainnet.g.alchemy.com/v2/WpfxSmp2Dng8vOSKRGIPWR0DCUamzs9i"); //Configuravel
            /*
             * 
             * CREATE TABLE SolanaRPC(
	                ID BsonId,
	                AccountUsing VARCHAR(250),
	                UrlRpc VARCHAR(500),
	                WebSocket VARCHAR(500),
	                ApyKey VARCHAR(200),
	                AppName VARCHAR(500),
	                IsActive BIT,
	                UsingDays INT
	                PRIMARY KEY(ID)
                );
             */
        }
        public async Task<List<SignatureRPCResponse>?> ExecuteRecoverySignaturesAsync(SignatureRPCRequest request)
        {
            var response = (HttpResponseMessage?)null!;
            var responseBody = string.Empty;
            /*
             */
            var data = "{\"method\": \"getSignaturesForAddress\",\"params\": [\"{{WalletHash}}\",{\"encoding\": \"json\",\"maxSupportedTransactionVersion\": 0,\"limit\": 1000,\"until\" : \"{{LastSignature}}\"}],\"jsonrpc\": \"2.0\",\"id\": \"{{id}}\"}";
            data = data.Replace("{{WalletHash}}", request.WalletHash);
            data = data.Replace("{{id}}", request.TransactionId);
            data = data.Replace("{{LastSignature}}", request.LastSignature);
            /*
             * 
             *  var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);
                var responseBody = await response.Content.ReadAsStringAsync();
             */
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("", content);
            responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                var jsonParsed = JsonConvert.DeserializeObject<TransacionRPC>(responseBody);
                return jsonParsed?.Result?.Select(x => new SignatureRPCResponse { WalletHash = request.WalletHash, Signature = x!.Signature, BlockTime = x.BlockTime, Err = x.Err, ConfirmationStatus = x.ConfirmationStatus }).ToList();
            }
            throw new Exception(responseBody);
        }
    }
}
