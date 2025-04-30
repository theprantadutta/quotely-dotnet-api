namespace quotely_dotnet_api.BackgroundJobs;

public class GetAiGenerateAuthorJob
{
    //     public async Task Invoke()
    // {
    //     _logger.LogInformation(
    //         "Starting AI-powered GetAllAuthorJob at {StartTime}...",
    //         DateTime.Now.ToCustomLogDateFormat()
    //     );
    //
    //     try
    //     {
    //         await Lock.WaitAsync();
    //
    //         // 1. Define generation parameters
    //         var batchSize = 20; // Authors per batch
    //         var topics = new[] { "philosophy", "literature", "science", "politics" }; // For diverse authors
    //
    //         // 2. Generate author batches
    //         foreach (var topic in topics)
    //         {
    //             _logger.LogInformation("Generating authors for topic: {Topic}", topic);
    //
    //             var prompt = $"""
    //                 Generate {batchSize} notable authors in {topic} with these details:
    //                 - Full name (standardized format)
    //                 - 50-word professional bio
    //                 - 10-word description
    //                 - Wikipedia-style slug (no special chars)
    //                 - Their most famous work
    //                 - Estimated quote count
    //                 - Era/century they belonged to
    //
    //                 Return JSON format:
    //                 {{
    //                     "authors": [
    //                         {{
    //                             "name": "Friedrich Nietzsche",
    //                             "bio": "German philosopher...",
    //                             "description": "Existentialist philosopher",
    //                             "slug": "friedrich-nietzsche",
    //                             "link": "https://en.wikipedia.org/wiki/Friedrich_Nietzsche",
    //                             "famous_work": "Thus Spoke Zarathustra",
    //                             "quote_count": 1200,
    //                             "era": "19th century"
    //                         }}
    //                     ]
    //                 }}
    //                 """;
    //
    //             var authorsBatch = await GenerateAuthorsWithOpenAI(prompt);
    //
    //             // 3. Process each generated author
    //             foreach (var authorDto in authorsBatch)
    //             {
    //                 await ProcessAuthor(authorDto);
    //                 await Task.Delay(500); // Rate limiting
    //             }
    //
    //             await Task.Delay(2000); // Between topics
    //         }
    //
    //         _logger.LogInformation("AI-powered GetAllAuthorJob completed successfully");
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, "AI author generation failed");
    //         // Don't throw to continue weekly jobs
    //     }
    //     finally
    //     {
    //         Lock.Release();
    //         _logger.LogInformation(
    //             "Finished at {EndTime}",
    //             DateTime.Now.ToCustomLogDateFormat()
    //         );
    //     }
    // }
    //
    // private async Task<List<AuthorDto>> GenerateAuthorsWithOpenAI(string prompt)
    // {
    //     try
    //     {
    //         var requestBody = new
    //         {
    //             model = "gpt-4",
    //             messages = new[] { new { role = "user", content = prompt } },
    //             response_format = new { type = "json_object" }
    //         };
    //
    //         var response = await _openAiClient.PostAsync("chat/completions", requestBody);
    //         var jsonResponse = JObject.Parse(response);
    //
    //         return jsonResponse["authors"].ToObject<List<AuthorDto>>();
    //     }
    //     catch
    //     {
    //         _logger.LogWarning("Failed to generate authors batch");
    //         return new List<AuthorDto>();
    //     }
    // }
    //
    // private async Task ProcessAuthor(AuthorDto authorDto)
    // {
    //     // 1. Create consistent identifier
    //     var authorHash = GenerateAuthorHash(authorDto.Name, authorDto.FamousWork);
    //
    //     // 2. Check for existing records
    //     var existingAuthor = await _appDbContext.Authors
    //         .FirstOrDefaultAsync(a => a.Slug == authorDto.Slug ||
    //                                  a.NameHash == authorHash);
    //
    //     // 3. Get Wikipedia image if available
    //     var imageUrl = await _utilityService.GetWikipediaThumbnailAsync(authorDto.Link);
    //
    //     if (existingAuthor == null)
    //     {
    //         var newAuthor = new Author
    //         {
    //             Id = Guid.NewGuid().ToString(),
    //             Name = authorDto.Name,
    //             NameHash = authorHash,
    //             Bio = authorDto.Bio,
    //             Slug = authorDto.Slug,
    //             Description = authorDto.Description,
    //             Link = authorDto.Link,
    //             ImageUrl = imageUrl,
    //             QuoteCount = authorDto.QuoteCount,
    //             FamousWork = authorDto.FamousWork,
    //             Era = authorDto.Era,
    //             IsAiGenerated = true,
    //             DateAdded = DateTime.UtcNow,
    //             DateModified = DateTime.UtcNow
    //         };
    //         await _appDbContext.AddAsync(newAuthor);
    //     }
    //     else
    //     {
    //         // Update existing but preserve original metadata
    //         existingAuthor.Bio = authorDto.Bio;
    //         existingAuthor.Description = authorDto.Description;
    //         existingAuthor.ImageUrl = imageUrl ?? existingAuthor.ImageUrl;
    //         existingAuthor.DateModified = DateTime.UtcNow;
    //     }
    //
    //     await _appDbContext.SaveChangesAsync();
    // }
    //
    // private string GenerateAuthorHash(string name, string famousWork)
    // {
    //     var normalized = $"{name.Trim().ToLower()}|{famousWork.Trim().ToLower()}";
    //     using var sha256 = SHA256.Create();
    //     var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(normalized));
    //     return BitConverter.ToString(hashBytes).Replace("-", "");
    // }
}
