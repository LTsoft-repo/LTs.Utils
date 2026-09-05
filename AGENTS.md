# Repository Rules

These rules apply to the entire repository.

## Agent Base Rules

- Always treat this file (`AGENTS.md`) as the base rule set for every agent session in this repository.
- Read `AGENTS.md` before making edits, and follow its conventions unless the user explicitly overrides them.
- When other guidance conflicts with `AGENTS.md`, prefer `AGENTS.md` for repository-specific decisions.

## Before Editing

- Read the relevant code first and follow the existing structure.
- Check `git status --short` before edits and do not overwrite or revert user changes.
- Keep edits scoped to the user request. Do not perform broad formatting, line-ending normalization, or cleanup outside touched files unless the user explicitly asks for it.
- If a request mentions style, architecture, project layout, or formatting, inspect the repository ReSharper settings before changing files.

## Style

- ReSharper settings are the source of truth for C# style. Do not treat `.editorconfig` as the primary style source.
- Check the repository solution settings file (for example `*.sln.DotSettings` under `src\`).
- Check shared LTsoft ReSharper settings under `D:\Repositories\LTsoft\CodeGuidelines` when available.
- Use CRLF line endings for text files.
- Do not leave empty lines at the end of files.
- Files do not have a blank line at the end.
- End-of-line is always CRLF (`\r\n`).
- Do not add broad EOF or line-ending churn to unrelated files.
- Use 4 spaces for indentation.
- Keep C# using directives outside namespaces.
- If a C# file contains a class with a constructor, add `#pragma warning disable IDE0290` between the using directives and the namespace declaration.
- Preserve the LTsoft C# spacing style, including spaces inside method call/declaration parentheses and square brackets.
- Prefer expression-bodied members where the existing style and ReSharper settings call for them.
- Private fields use camelCase without prefixes.
- Test classes do not require XML summaries.
- Production public types and members need XML summaries when documentation generation is enabled.
- Put each class, record, interface, enum, or struct in its own file. The file name must match the type name.
- Do not place multiple types in the same file.
- The only exception is `partial` types split across files, typically when one part is source-generated and extends the hand-written typed part.
- Do not use nested helper types in production or test code; place test helpers under `<test project>\Infrastructure` instead.
- Order class and record members in this sequence: properties, fields, constructors, methods.
- Within each member group, order by accessibility: public, protected, private.
- Do not add nested classes or records unless explicitly requested. When nested types are required, place them after all other members and order them public, protected, private.

## Code Design

- Any code must follow SOLID principles.

## LTsoft Libraries

- Prefer classes, extension methods, and comparers from LTsoft NuGet packages (`LTs.Utils`, `LTs.Json`, `LTs.TestUtils`, `LTs.Web`, and related LTsoft packages) before introducing local helpers.
- Search existing LTsoft packages and sibling repositories for an existing utility before adding duplicate implementations in this repository.
- For `JToken` comparisons in tests, use `BeSameJsonAs` from `LTs.TestUtils.FluentAssertions` or `JTokenEqualityComparer` from `Newtonsoft.Json.Linq` with `BeEquivalentTo`, not custom comparers.
- For collection comparisons in tests, use `ContainExactlyEquivalent`, `ContainEquivalentSubset`, or `NotContainEquivalentInSubset` from `LTs.TestUtils.FluentAssertions`. Do not use `BeEquivalentTo` on collections.

## JSON

- Prefer `LTs.Json` extension methods for JSON parsing and serialization (`ParseAsJToken`, `ToJson`).
- `Newtonsoft.Json` (`JToken`, `JObject`, `JToken.FromObject`) is the underlying JSON model when `LTs.Json` does not provide a helper.
- Do not add `System.Text.Json` usage in new production code unless an existing component already depends on it.
- Prefer `JToken` for tool arguments, planner payloads, and JSON schema metadata at tool boundaries.
- Use the same preview version of `LTs.Json` as other LTsoft packages already referenced in the solution.

## Solution And Project Layout

- All project filesystem folders live directly under `src\`.
- Project filesystem folder names must match project names exactly.
- Visual Studio solution folders are logical only and are represented in the solution file (for example `src\*.slnx`).
- Do not rename projects unless explicitly asked.
- Main project names use the form `<Prefix>.<Area>`.
- Area folders inside projects are real code folders, not separate projects, unless explicitly requested.
- Test project names use lowercase `test`, not `Test` or `Tests`.
- Integration test project names use lowercase `test.integration`.
- Test projects belong under the corresponding solution folder's `tests` logical folder.
- Test projects still live directly under `src\` physically.

## Project File Rules

- Non-test projects include `<GenerateDocumentationFile>True</GenerateDocumentationFile>`.
- Test projects do not include `<GenerateDocumentationFile>`.
- Each project folder should contain `GlobalSuppressions.cs`.
- Use the latest preview version of LTsoft NuGet packages already used by the solution.
- Do not update unrelated third-party packages unless explicitly asked.
- Do not duplicate `<PackageReference>` entries in `*.test` or `*.test.integration` projects when the same package is already referenced by a production project under test through `<ProjectReference>`.
- Test projects should reference test-only packages (`LTs.TestUtils`, `xunit`, `Moq`, `coverlet.collector`, `Microsoft.NET.Test.Sdk`) plus `<ProjectReference>` to the project(s) under test.
- If a test needs behavior from a production dependency, exercise it through the production project's public API instead of adding that package to the test project.

## Testing Rules

- Add or update unit tests in the matching test project for every production code change in the same pass. Do not defer test coverage to a follow-up.
- Test classes must always derive from `LTs.TestUtils.Tests.BaseTest`.
- Tests must always use FluentAssertions.
- Tests must use `BeEquivalentTo` when validating a single result or object.
- Do not use `BeEquivalentTo` on collections. Use `ContainExactlyEquivalent`, `ContainEquivalentSubset`, or `NotContainEquivalentInSubset` from `LTs.TestUtils.FluentAssertions` instead.
- When an object contains a collection property, assert the object with `BeEquivalentTo` (excluding the collection when needed) and assert the collection separately with the appropriate collection extension.
- Helper classes added only for tests must be internal classes under `<test project>\Infrastructure`, in their own file, not nested inside the test class.
- Keep Arrange, Act, and Assert as separate blocks in each test.
- Assign parsed, transformed, or extracted values to a local variable before asserting on them.
- The expression before `.Should()` must be a local variable or parameter name only. Do not chain method calls, property access, indexing, LINQ, casts, or other expressions directly into `.Should()`.
- Do not insert a blank line between a variable assignment and the assertion that follows it.
- Insert a blank line between separate assertion groups in the Assert block so each check reads as its own unit.
- Use FluentAssertions call style: `subject.Should().Be...()` or `collection.Should().ContainExactlyEquivalent(...)`. Do not add `ShouldBe...()` methods on the subject itself.
- Prefer combined FluentAssertions checks such as `NotBeNullOrEmpty()` instead of separate `NotBeNull()` and `NotBeEmpty()` (or `NotBeNullOrWhiteSpace()`) on the same subject.
- Add custom FluentAssertions extensions on the assertions object (for example `JTokenAssertions`), not on the subject type.

## LLM Rules

- The LLM area project owns provider-neutral LLM abstractions and provider implementations.
- Provider HTTP clients use `LTs.Web.HttpHandler`, not direct `HttpClient` usage.
- Unit tests for provider HTTP clients mock `HttpHandler`.
- Unit tests for provider-neutral LLM adapters mock the provider client abstraction.
- Tests that call a real local LLM server belong in the corresponding `test.integration` project.
- Use the model name already established by existing integration tests in the repository when adding or updating LLM integration tests.

## Core Rules

- Keep the core area project free of direct LLM provider dependencies unless explicitly requested.
- Core integration tests should exercise real core orchestration classes together rather than mocking the full agent loop.
- Use small test-only tools inside integration tests when needed to avoid external dependencies.

## Git And Release Notes

- When completing a set of changes, always provide the commit comment text in the format below, even if the user does not explicitly ask for it.
- Always append the commit bullet lines to `ReleaseNotes\v0.1.0.md` under `## Changes`.
- Use the LTsoft release notes style: title `# Version X.Y.Z`, one `## Changes` section, and a flat bullet list starting with imperative verbs (`Add`, `Update`, `Refactor`, and so on).
- Do not add milestone, area, or subsection headings to release notes files.
- Do not create a new `ReleaseNotes\vX.Y.Z.md` file or update `README.md` change logs until the user explicitly starts a new version release.
- Do not stage or commit files unless the user explicitly asks.

## Git Commit Messages

- Use this subject format: `1 - <concise summary>.`
- Start the summary with an imperative verb (for example: `Add`, `Update`, `Fix`, `Refactor`, `Introduce`).
- Keep the subject on one line and end it with a period.
- Add a blank line after the subject when extra detail is needed.
- Use `- ` bullet lines for notable changes; keep bullets short and specific.
- Match the tone and structure of recent commits in this repository.

Example:

```text
1 - Add integration tests and repository agent rules.

- Add an integration test project for the core area.
- Refactor unit tests to use Infrastructure helpers.
- Register the new test project in the solution.
```

## Verification

- Restore after package or project changes.
- Build the solution after code or project changes.
- Run the narrowest relevant tests for the change.
- For integration tests that depend on local services, run them only when they are relevant or explicitly requested.
- Before final response, verify edited files have CRLF line endings and no empty lines at EOF.
- Report any tests not run and why.