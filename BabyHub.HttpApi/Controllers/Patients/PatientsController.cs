using BabyHub.Application.Contracts.Patients;
using BabyHub.Domain.Shared.Constants;
using BabyHub.Domain.Shared.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BabyHub.HttpApi.Controllers.Patients
{
    /// <summary>
    /// Provides CRUD operations for patients.
    /// </summary>
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientAppService _patientAppService;

        public PatientsController(IPatientAppService patientAppService)
        {
            _patientAppService = patientAppService;
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="input">Patient creation data.</param>
        /// <returns>Identifier of the created patient.</returns>
        /// <response code="201">Patient successfully created.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateAsync(
            [FromBody] PatientCreateDto input,
            [FromServices] IValidator<PatientCreateDto> validator)
        {
            var validation = await validator.ValidateAsync(input);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            }

            var id = await _patientAppService.CreateAsync(input);
            return CreatedAtRoute(
                routeName: "GetPatientById",
                routeValues: new { id },
                value: id);
        }

        /// <summary>
        /// Updates an existing patient (full replacement).
        /// All fields are overwritten with the provided values.
        /// Nullable fields set to null will be cleared.
        /// </summary>
        /// <param name="id">Patient identifier.</param>
        /// <param name="input">Complete patient data to replace the existing record.</param>
        /// <response code="204">Patient successfully updated.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Patient not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            [FromBody] PatientUpdateDto input,
            [FromServices] IValidator<PatientUpdateDto> validator)
        {
            var validation = await validator.ValidateAsync(input);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            }

            await _patientAppService.UpdateAsync(id, input);
            return NoContent();
        }

        /// <summary>
        /// Deletes a patient.
        /// </summary>
        /// <param name="id">Patient identifier.</param>
        /// <response code="204">Patient successfully deleted.</response>
        /// <response code="404">Patient not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _patientAppService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Gets a patient by identifier.
        /// </summary>
        /// <param name="id">Patient identifier.</param>
        /// <returns>Patient data.</returns>
        /// <response code="200">Patient found.</response>
        /// <response code="404">Patient not found.</response>
        [HttpGet("{id}", Name = "GetPatientById")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> GetAsync(Guid id)
        {
            var patient = await _patientAppService.GetAsync(id);
            return Ok(patient);
        }

        /// <summary>
        /// Gets a list of patients, optionally filtered by birth date.
        /// Supports FHIR date search prefixes: eq, ne, gt, lt, ge, le, sa, eb, ap.
        /// </summary>
        /// <param name="birthDate">
        /// Optional birth date filter in FHIR format, e.g. "eq2013-01-14", "ge2010-01-01T00:00:00".
        /// If no prefix is specified, "eq" is assumed.
        /// </param>
        /// <param name="count">
        /// Optional. Maximum number of patients to return.
        /// Must be greater than 0. If not specified, all matching patients are returned.
        /// </param>
        /// <returns>List of patients matching the filter.</returns>
        /// <response code="200">List returned successfully.</response>
        /// <response code="400">Invalid birthDate format, unsupported operator, or invalid count value.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PatientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetListAsync(
            [FromQuery] string? birthDate = null,
            [FromQuery] int? count = null)
        {
            try
            {
                if (count.HasValue && count.Value <= 0)
                {
                    return BadRequest("count must be greater than 0.");
                }

                var patients = await _patientAppService.GetListAsync(birthDate, count);
                return Ok(patients);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
