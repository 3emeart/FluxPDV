using System.Net.Http.Json;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MiniMercadoSaas.Application.DTO.Response;

namespace MiniMercadoSaas.Application.Services;

public class MercadoPagoService : IPagamentoService
{
    private readonly string _accessToken;
    private readonly HttpClient _httpClient;

    public MercadoPagoService(string accessToken)
    {
        _accessToken = accessToken;
        _httpClient = new HttpClient();
    }

    public async Task<PixResponse> GerarPagamentoPix(Guid vendaId, decimal valor)
    {
        var url = "https://api.mercadopago.com/v1/payments";
        
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
        _httpClient.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

        var body = new
        {
            transaction_amount = valor,
            description = $"Venda MiniMercado #{vendaId}",
            payment_method_id = "pix",
            external_reference = vendaId.ToString(),
            payer = new
            {
                email = "soniasilva3marias@gmail.com",
                first_name = "Sonia",
                last_name = "Silva",
                identification = new 
                {
                    type = "CPF",
                    number = "09486458499"
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(url, body);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Erro MP: {response.StatusCode} | Detalhes: {content}");
        }

        using var json = System.Text.Json.JsonDocument.Parse(content);
        var transactionData = json.RootElement.GetProperty("point_of_interaction").GetProperty("transaction_data");

        return new PixResponse
        {
            CopyAndPaste = transactionData.GetProperty("qr_code").GetString() ?? "",
            QrCodeBase64 = transactionData.GetProperty("qr_code_base64").GetString() ?? "",
            ExternalReference = vendaId.ToString()
        };
    }
}