// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;

namespace Tests.CodeOfChaos.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class StringCaseExtensionTests {
    
    // Tests for ToCamelCase
    [Test]
    [Arguments("a", "a")]
    [Arguments("A", "a")]
    [Arguments("aA", "aA")]
    [Arguments("aa", "aa")]
    [Arguments("aaA", "aaA")]
    [Arguments("a1a", "a1A")]
    [Arguments("AlphaBeta", "alphaBeta")]
    [Arguments("AlphaBetaGamma", "alphaBetaGamma")]
    [Arguments("AlphaBetaGammaDelta", "alphaBetaGammaDelta")]
    [Arguments("snake_case", "snakeCase")]
    [Arguments("snake_case_with_numbers_123", "snakeCaseWithNumbers123")]
    [Arguments("snake_case_with_numbers_123_and_more", "snakeCaseWithNumbers123AndMore")]
    [Arguments("period.case", "periodCase")]
    [Arguments("period.case.with.numbers.123", "periodCaseWithNumbers123")]
    [Arguments("period.case.with.numbers.123.and.more", "periodCaseWithNumbers123AndMore")]
    [Arguments("kebab-case", "kebabCase")]
    [Arguments("kebab-case-with-numbers-123", "kebabCaseWithNumbers123")]
    [Arguments("kebab-case-with-numbers-123-and-more", "kebabCaseWithNumbers123AndMore")]
    public async Task ToCamelCase_ShouldWork(string input, string expected) {
        // Arrange
        
        // Act
        var result = input.ToCamelCase();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    // Tests for ToPascalCase
    [Test]
    [Arguments("a", "A")]
    [Arguments("A", "A")]
    [Arguments("aA", "AA")]
    [Arguments("aa", "Aa")]
    [Arguments("aaA", "AaA")]
    [Arguments("a1a", "A1A")]
    [Arguments("AlphaBeta", "AlphaBeta")]
    [Arguments("AlphaBetaGamma", "AlphaBetaGamma")]
    [Arguments("AlphaBetaGammaDelta", "AlphaBetaGammaDelta")]
    [Arguments("snake_case", "SnakeCase")]
    [Arguments("snake_case_with_numbers_123", "SnakeCaseWithNumbers123")]
    [Arguments("snake_case_with_numbers_123_and_more", "SnakeCaseWithNumbers123AndMore")]
    [Arguments("period.case", "PeriodCase")]
    [Arguments("period.case.with.numbers.123", "PeriodCaseWithNumbers123")]
    [Arguments("period.case.with.numbers.123.and.more", "PeriodCaseWithNumbers123AndMore")]
    [Arguments("kebab-case", "KebabCase")]
    [Arguments("kebab-case-with-numbers-123", "KebabCaseWithNumbers123")]
    [Arguments("kebab-case-with-numbers-123-and-more", "KebabCaseWithNumbers123AndMore")]
    public async Task ToPascalCase_ShouldWork(string input, string expected) {
        // Arrange
        
        // Act
        var result = input.ToPascalCase();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    // Tests for ToKebabCase
    [Test]
    [Arguments("a", "a")]
    [Arguments("A", "a")]
    [Arguments("aA", "a-a")]
    [Arguments("aa", "aa")]
    [Arguments("aaA", "aa-a")]
    [Arguments("a1a", "a-1-a")]
    [Arguments("AlphaBeta", "alpha-beta")]
    [Arguments("AlphaBetaGamma", "alpha-beta-gamma")]
    [Arguments("AlphaBetaGammaDelta", "alpha-beta-gamma-delta")]
    [Arguments("snake_case", "snake-case")]
    [Arguments("snake_case_with_numbers_123", "snake-case-with-numbers-123")]
    [Arguments("snake_case_with_numbers_123_and_more", "snake-case-with-numbers-123-and-more")]
    [Arguments("period.case", "period-case")]
    [Arguments("period.case.with.numbers.123", "period-case-with-numbers-123")]
    [Arguments("period.case.with.numbers.123.and.more", "period-case-with-numbers-123-and-more")]
    [Arguments("kebab-case", "kebab-case")]
    [Arguments("kebab-case-with-numbers-123", "kebab-case-with-numbers-123")]
    [Arguments("kebab-case-with-numbers-123-and-more", "kebab-case-with-numbers-123-and-more")]
    public async Task ToKebabCase_ShouldWork(string input, string expected) {
        // Arrange
        
        // Act
        var result = input.ToKebabCase();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    // Tests for ToSnakeCase
    [Test]
    [Arguments("a", "a")]
    [Arguments("A", "a")]
    [Arguments("aA", "a_a")]
    [Arguments("aa", "aa")]
    [Arguments("aaA", "aa_a")]
    [Arguments("a1a", "a_1_a")]
    [Arguments("AlphaBeta", "alpha_beta")]
    [Arguments("AlphaBetaGamma", "alpha_beta_gamma")]
    [Arguments("AlphaBetaGammaDelta", "alpha_beta_gamma_delta")]
    [Arguments("snake_case", "snake_case")]
    [Arguments("snake_case_with_numbers_123", "snake_case_with_numbers_123")]
    [Arguments("snake_case_with_numbers_123_and_more", "snake_case_with_numbers_123_and_more")]
    [Arguments("period.case", "period_case")]
    [Arguments("period.case.with.numbers.123", "period_case_with_numbers_123")]
    [Arguments("period.case.with.numbers.123.and.more", "period_case_with_numbers_123_and_more")]
    [Arguments("kebab-case", "kebab_case")]
    [Arguments("kebab-case-with-numbers-123", "kebab_case_with_numbers_123")]
    [Arguments("kebab-case-with-numbers-123-and-more", "kebab_case_with_numbers_123_and_more")]
    public async Task ToSnakeCase_ShouldWork(string input, string expected) {
        // Arrange
        
        // Act
        var result = input.ToSnakeCase();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    // Tests for ToPeriodSeparatedCase
    [Test]
    [Arguments("a", "a")]
    [Arguments("A", "a")]
    [Arguments("aA", "a.a")]
    [Arguments("aa", "aa")]
    [Arguments("aaA", "aa.a")]
    [Arguments("a1a", "a.1.a")]
    [Arguments("AlphaBeta", "alpha.beta")]
    [Arguments("AlphaBetaGamma", "alpha.beta.gamma")]
    [Arguments("AlphaBetaGammaDelta", "alpha.beta.gamma.delta")]
    [Arguments("snake_case", "snake.case")]
    [Arguments("snake_case_with_numbers_123", "snake.case.with.numbers.123")]
    [Arguments("snake_case_with_numbers_123_and_more", "snake.case.with.numbers.123.and.more")]
    [Arguments("period.case", "period.case")]
    [Arguments("period.case.with.numbers.123", "period.case.with.numbers.123")]
    [Arguments("period.case.with.numbers.123.and.more", "period.case.with.numbers.123.and.more")]
    [Arguments("kebab-case", "kebab.case")]
    [Arguments("kebab-case-with-numbers-123", "kebab.case.with.numbers.123")]
    [Arguments("kebab-case-with-numbers-123-and-more", "kebab.case.with.numbers.123.and.more")]
    public async Task ToPeriodSeparatedCase_ShouldWork(string input, string expected) {
        // Arrange
        
        // Act
        var result = input.ToPeriodSeparatedCase();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}