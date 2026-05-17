namespace MiniMercadoSaas.Application.DTO.Response;

public class PixResponse
{
    public string QrCodeBase64 { get; set; } = string.Empty;
    public string CopyAndPaste { get; set; } = string.Empty;
    public string ExternalReference { get; set; } = string.Empty;
}