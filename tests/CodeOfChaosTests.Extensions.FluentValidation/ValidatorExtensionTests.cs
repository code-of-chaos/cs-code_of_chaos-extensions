// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.FluentValidation;
using CodeOfChaosTests.Extensions.FluentValidation.Assets;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace CodeOfChaosTests.Extensions.FluentValidation;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ValidatorExtensionTests {
    private static readonly TestModel ValidInstance = new() { Id = 1, Name = "Valid" };
    private static readonly TestModel InvalidInstance = new() { Id = 0, Name = "" };
    private static readonly List<ValidationFailure> ValidationFailures = new() {
        new ValidationFailure("Id", "Id must be greater than zero."),
        new ValidationFailure("Name", "Name cannot be empty.")
    };
    private readonly IValidator<TestModel> _mockedValidator = SetupValidator().Object;

    private static Mock<IValidator<TestModel>> SetupValidator() {
        var mockValidator = new Mock<IValidator<TestModel>>();
        mockValidator
            .Setup(v => v.Validate(ValidInstance))
            .Returns(new ValidationResult());

        mockValidator
            .Setup(v => v.Validate(InvalidInstance))
            .Returns(new ValidationResult(ValidationFailures));

        mockValidator
            .Setup(v => v.ValidateAsync(ValidInstance, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        mockValidator
            .Setup(v => v.ValidateAsync(InvalidInstance, CancellationToken.None))
            .ReturnsAsync(new ValidationResult(ValidationFailures));

        return mockValidator;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task TryValidate_ValidInstance_ReturnsTrueAndNoFailures() {
        bool result = _mockedValidator.TryValidate(ValidInstance, out List<ValidationFailure> failures);

        await Assert.That(result).IsTrue();
        await Assert.That(failures).IsEmpty();
    }

    [Test]
    public async Task TryValidate_InvalidInstance_ReturnsFalseAndFailures() {
        bool result = _mockedValidator.TryValidate(InvalidInstance, out List<ValidationFailure> failures);

        await Assert.That(result).IsFalse();
        await Assert.That(failures).IsEquivalentTo(ValidationFailures);
    }

    [Test]
    public async Task ThrowIfInvalid_InvalidInstance_ThrowsValidationException() {
        var exception = Assert.Throws<ValidationException>(() => _mockedValidator.ThrowIfInvalid(InvalidInstance));

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception.Errors).IsEquivalentTo(ValidationFailures);
    }

    [Test]
    public async Task ValidateAndGetErrorMessages_InvalidInstance_ReturnsErrorMessages() {
        List<string> errors = _mockedValidator.ValidateAndGetErrorMessages(InvalidInstance).ToList();

        await Assert.That(errors).IsNotNull()
            .And.IsEquivalentTo(ValidationFailures.Select(x => x.ErrorMessage));
    }

    [Test]
    public async Task ValidateAndGetErrorMessages_ValidInstance_ReturnsNoMessages() {
        IEnumerable<string> errors = _mockedValidator.ValidateAndGetErrorMessages(ValidInstance);

        await Assert.That(errors).IsNotNull().And.IsEmpty();
    }

    [Test]
    public async Task ValidateAndGetErrors_InvalidInstance_ReturnsValidationFailures() {
        List<ValidationFailure> failures = _mockedValidator.ValidateAndGetErrors(InvalidInstance);

        await Assert.That(failures).IsNotNull();
        await Assert.That(failures).IsEquivalentTo(ValidationFailures);
    }

    [Test]
    public async Task ValidateAndGetErrors_ValidInstance_ReturnsEmptyList() {
        List<ValidationFailure> failures = _mockedValidator.ValidateAndGetErrors(ValidInstance);

        await Assert.That(failures).IsNotNull()
            .And.IsEmpty();
    }

    [Test]
    public async Task ValidateOrDefault_InvalidInstance_ReturnsDefaultValue() {
        var defaultInstance = new TestModel { Id = -1, Name = "Default" };

        TestModel result = _mockedValidator.ValidateOrDefault(InvalidInstance, defaultValueFactory: () => defaultInstance);

        await Assert.That(result).IsNotNull()
            .And.IsEqualTo(defaultInstance);
    }

    [Test]
    public async Task ValidateOrDefault_ValidInstance_ReturnsOriginalInstance() {
        TestModel result = _mockedValidator.ValidateOrDefault(ValidInstance, defaultValueFactory: () => new TestModel());

        await Assert.That(result).IsNotNull()
            .And.IsEqualTo(ValidInstance);
    }

    [Test]
    public async Task ValidateOrDefaultAsync_InvalidInstance_ReturnsDefaultValue() {
        var defaultInstance = new TestModel { Id = -1, Name = "Default" };

        TestModel? result = await _mockedValidator.ValidateOrDefaultAsync(InvalidInstance, defaultValueFactory: () => ValueTask.FromResult(defaultInstance));

        await Assert.That(result).IsNotNull()
            .And.IsEqualTo(defaultInstance);
    }

    [Test]
    public async Task ValidateOrDefaultAsync_ValidInstance_ReturnsOriginalInstance() {
        TestModel? result = await _mockedValidator.ValidateOrDefaultAsync(ValidInstance);

        await Assert.That(result).IsNotNull()
            .And.IsEqualTo(ValidInstance);
    }
}
