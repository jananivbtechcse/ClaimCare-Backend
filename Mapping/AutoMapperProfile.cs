using AutoMapper;
using ClaimCare.Models;
using ClaimCare.DTOs.PatientDTO;
using ClaimCare.DTOs.ClaimDTO;
using ClaimCare.DTOs.ClaimDocumentDTO;
using ClaimCare.DTOs.InvoiceDTO;
using ClaimCare.DTOs.NotificationDTO;
using ClaimCare.DTOs.PaymentDTO;
using ClaimCare.DTOs.InsuranceCompanyDTO;
using ClaimCare.DTOs.HealthcareProviderDTO;
using ClaimCare.DTOs.InsurancePlanDTO;
using ClaimCare.DTOs.UserDTO;
using ClaimCare.DTOs;

namespace ClaimCare.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()

        
        {

         CreateMap<InsuranceCompany, InsuranceCompanyResponseDTO>();
                                

            CreateMap<UpdateInsuranceCompanyDTO, InsuranceCompany>()
    .ForMember(dest => dest.RegistrationNumber, opt => opt.Ignore());


            CreateMap<CreatePatientDTO, Patient>()
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Patient, PatientResponseDTO>();

               CreateMap<UpdatePatientDTO, Patient>()
    .ForMember(dest => dest.InsurancePlanId,
               opt => opt.Condition(src => src.InsurancePlanId.HasValue))
    .ForAllMembers(opts =>
        opts.Condition((src, dest, srcMember) => srcMember != null));

            // Patient -> Detail DTO
            CreateMap<Patient, PatientDetailDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.InsurancePlanName,
                    opt => opt.MapFrom(src => src.InsurancePlan.PlanName));

                CreateMap<Notification, NotificationResponseDTO>();

               //claim mapping
                CreateMap<CreateClaimDTO, Claim>()
                .ForMember(dest => dest.ClaimId, opt => opt.Ignore())
                .ForMember(dest => dest.ClaimNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.SubmissionDate, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedDate, opt => opt.Ignore())
                .ForMember(dest => dest.RejectionReason, opt => opt.Ignore());

          CreateMap<Claim, ClaimResponseDTO>()
    .ForMember(dest => dest.InvoiceNumber,
        opt => opt.MapFrom(src => src.Invoice.InvoiceNumber))
    .ForMember(dest => dest.TotalAmount,
        opt => opt.MapFrom(src => src.Invoice.TotalAmount));

        CreateMap<Claim, ClaimStatusResponseDTO>();

            CreateMap<CreateInvoiceDTO, Invoice>()
                .ForMember(dest => dest.InvoiceId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.TaxAmount, opt => opt.Ignore());

            // CreateMap<Invoice, InvoiceResponseDTO>()
            // .ForMember(dest => dest.PatientName,
            //     opt => opt.MapFrom(src => src.Patient.User.FullName))
            // .ForMember(dest => dest.ProviderName,
            //     opt => opt.MapFrom(src => src.Provider.HospitalName))
            // .ForMember(dest => dest.InvoiceDate,
            //     opt => opt.MapFrom(src => src.CreatedAt));
            //     CreateMap<InvoiceRequest, InvoiceRequestResponseDTO>()
            //     .ForMember(dest => dest.PatientName,
            //         opt => opt.MapFrom(src => src.Patient.User.FullName))

            //     .ForMember(dest => dest.ProviderName,
            //         opt => opt.MapFrom(src => src.Provider.User.FullName));


            CreateMap<Invoice, InvoiceResponseDTO>()
    .ForMember(dest => dest.PatientName,
        opt => opt.MapFrom(src => src.Patient.User.FullName))
    .ForMember(dest => dest.ProviderName,
        opt => opt.MapFrom(src => src.Provider.HospitalName))
    .ForMember(dest => dest.InvoiceDate,
        opt => opt.MapFrom(src => src.CreatedAt))
    .ForMember(dest => dest.PatientId,
        opt => opt.MapFrom(src => src.Patient.PatientId.ToString()))
    .ForMember(dest => dest.PatientAddress,
        opt => opt.MapFrom(src => src.Patient.Address))   // adjust to your Patient field name
    .ForMember(dest => dest.DueDate,
        opt => opt.MapFrom(src => src.DueDate));          // add DueDate to Invoice model if missing

        CreateMap<InvoiceRequest, InvoiceRequestResponseDTO>()
    .ForMember(dest => dest.PatientName,
        opt => opt.MapFrom(src => src.Patient.User.FullName))
    .ForMember(dest => dest.ProviderName,
        opt => opt.MapFrom(src => src.Provider.User.FullName));
        
            // =========================
            // Payment Mappings
            // =========================

            CreateMap<CreatePaymentDTO, Payment>()
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore());

            CreateMap<Payment, PaymentResponseDTO>()
    .ForMember(dest => dest.ClaimStatus,
        opt => opt.MapFrom(src => src.Claim.Status));

            // =========================
            // Insurance Company
            // =========================

            CreateMap<CreateInsuranceCompanyDTO, InsuranceCompany>()
                .ForMember(dest => dest.InsuranceCompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<InsuranceCompany, InsuranceCompanyResponseDTO>();


            // =========================
            // Healthcare Provider
            // =========================

            CreateMap<CreateHealthcareProviderDTO, HealthcareProvider>()
                .ForMember(dest => dest.ProviderId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<HealthcareProvider, HealthcareProviderResponseDTO>()
            .ForMember(dest => dest.HospitalName,
                opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.User.Email));

           // Create
CreateMap<CreateInsurancePlanDTO, InsurancePlan>()
    .ForMember(dest => dest.InsurancePlanId, opt => opt.Ignore());

       CreateMap<InvoiceRequest, InvoiceRequestResponseDTO>()
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient.User.FullName))

                .ForMember(dest => dest.ProviderName,
                    opt => opt.MapFrom(src => src.Provider.User.FullName));
// Update
CreateMap<UpdateInsurancePlanDto, InsurancePlan>()
    .ForMember(dest => dest.InsurancePlanId, opt => opt.Ignore())
    .ForMember(dest => dest.InsuranceCompanyId, opt => opt.Ignore());

// Response
CreateMap<InsurancePlan, InsurancePlanResponseDTO>()
    .ForMember(dest => dest.InsuranceCompanyName, opt => opt.MapFrom(src => src.InsuranceCompany.CompanyName));

    CreateMap<UpdateClaimStatusDTO, Claim>()
    .ForMember(dest => dest.Status, 
        opt => opt.MapFrom(src => src.Status))
    .ForMember(dest => dest.RejectionReason, 
        opt => opt.MapFrom(src => src.RejectionReason));

    
        CreateMap<ClaimDocument, ClaimDocumentResponseDTO>();

       CreateMap<User, UserResponseDTO>()
    .ForMember(dest => dest.UserId,
        opt => opt.MapFrom(src => src.UserId))
    .ForMember(dest => dest.Role,
        opt => opt.MapFrom(src => src.Role.RoleName));




        CreateMap<CreatePaymentDTO, Payment>();
 
            // Payment → PaymentResponseDTO
            // Map ClaimNumber from the related Claim navigation property
            // Map TransactionReference directly from the Payment entity
            CreateMap<Payment, PaymentResponseDTO>()
                .ForMember(dest => dest.ClaimNumber,
                    opt => opt.MapFrom(src => src.Claim != null ? src.Claim.ClaimNumber : null))
                .ForMember(dest => dest.ClaimStatus,
                    opt => opt.MapFrom(src => src.Claim != null ? src.Claim.Status : null))
                .ForMember(dest => dest.TransactionReference,
                    opt => opt.MapFrom(src => src.TransactionReference));




                    CreateMap<Claim, ClaimResponseDTO>()
    .ForMember(dest => dest.InvoiceNumber,
               opt => opt.MapFrom(src => src.Invoice != null 
                   ? src.Invoice.InvoiceNumber 
                   : null));
        }
       

       



     
    }
}