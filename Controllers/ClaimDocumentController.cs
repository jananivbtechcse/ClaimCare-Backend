using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClaimCare.Services.Interfaces;
using ClaimCare.Models;
using ClaimCare.DTOs.ClaimDocumentDTO;
using AutoMapper;

namespace ClaimCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimDocumentController : ControllerBase
    {
        private readonly IClaimDocumentRepository _repository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public ClaimDocumentController(
            IClaimDocumentRepository repository,
            IWebHostEnvironment environment,
            IMapper mapper)
        {
            _repository = repository;
            _environment = environment;
            _mapper = mapper;
        }

  
        [HttpPost("upload")]
        [Authorize(Roles = "Patient,HealthcareProvider")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadClaimDocumentDTO dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("File is required");

            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "claim-documents");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var document = new ClaimDocument
            {
                ClaimId = dto.ClaimId,
                FileName = dto.File.FileName,
                FilePath = "/claim-documents/" + fileName,
                UploadedDate = DateTime.UtcNow
            };

            await _repository.AddDocument(document);
            await _repository.SaveAsync();

            return Ok("Document uploaded successfully");
        }

       
        [HttpGet("claim/{claimId}")]
        [Authorize(Roles = "Patient,InsuranceCompany,HealthcareProvider")]
        public async Task<IActionResult> GetDocuments(int claimId)
        {
            var documents = await _repository.GetDocumentsByClaimId(claimId);

            var result = _mapper.Map<List<ClaimDocumentResponseDTO>>(documents);

            return Ok(result);
        }
    }
}