namespace learnApi.DTOs
{
    public record struct QuotationCreateDto(UserCreateDto user,List<ItemCreateDto> items);
}