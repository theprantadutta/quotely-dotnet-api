﻿using quotely_dotnet_api.Dtos;

namespace quotely_dotnet_api.Interfaces;

public interface IQuoteService
{
    Task<QuoteResponseDto> GetAllQuotes(int pageNumber, int pageSize, bool getAllRows);
}