# Repository Rules

These rules apply to the entire repository.

## Agent Base Rules

- Always treat this file (`AGENTS.md`) as the base rule set for every agent session in this repository.
- Read `AGENTS.md` before making edits, and follow its conventions unless the user explicitly overrides them.
- When other guidance conflicts with `AGENTS.md`, prefer `AGENTS.md` for repository-specific decisions.

## Rules Authoring

- Rules in `.cursor/rules/` and reusable guidance in `AGENTS.md` must be **generic and portable** unless the user explicitly asks for repository- or solution-specific rules.
- Use placeholders (`<Prefix>`, `HostConfiguration`, `IApplicationService`) in rules — never production type, project, or `appsettings` section names from this solution.
- When documenting a pattern from existing code, extract the generic shape first, then name placeholders. Do not copy current implementation names into rules.
- Do not create solution maps, project inventories, or area-specific rule files (for example `*-project.mdc`) unless the user explicitly requests them.
- Executable and host rules must be copyable to another LTsoft repo with only placeholder renames and a new `*.Host` implementation.


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
- Do not seal classes or records unless the user explicitly requests it.
- Do not place multiple types in the same file.
- The only exception is `partial` types split across files, typically when one part is source-generated and extends the hand-written typed part.
- Do not use nested helper types in production or test code; place test helpers under `<test project>\Infrastructure` instead.
- Order class and record members in this sequence: properties, fields, constructors, methods.
- Within each member group, order by accessibility: public, protected, private.
- Do not add nested classes or records unless explicitly requested. When nested types are required, place them after all other members and order them public, protected, private.

## Entry Points

- Never use top-level statements for APIs, Function Apps, console apps, workers, or any executable entry point.
- Always use an explicit `Program` class with a `Main` method.
- Use a public `Main` method and public static registration methods on `Program` when tests or host bootstrap need direct access.
- Follow `.cursor/rules/host-composition.mdc` for host bootstrap, configuration, and Autofac registration.
- Follow `.cursor/rules/configuration.mdc` for typed configuration records, loaders, and appsettings conventions.
- Follow `.cursor/rules/host-api.mdc` when creating or editing a Web API host executable (`<Prefix>.Host.Api`).
- Follow `.cursor/rules/host-console.mdc` when creating or editing a console host executable (`<Prefix>.Host.Console`).
- Put startup in `Program.Main`, extracting setup into public static methods on `Program` when needed.
- Do not delegate `Main` to a separate application startup class.

## Code Design

- Any code must follow SOLID principles.
- Keep separation of concerns: entry points and composition roots decide **what** to wire, not **how** each piece is built or loaded.
- `Program` and similar bootstrap code should call registration helpers instead of inlining configuration loading, binding, transformation, or service wiring details.
- `Program` should register only host-bootstrap concerns the entry point itself needs, such as logging bootstrap configuration. Delegate feature-owned configuration and services to registration extensions colocated with that feature.
- Register dependencies through abstractions in Autofac and similar containers. Use `.As<IAbstraction>()` instead of registering concrete types directly when an abstraction exists.
- Use enums for fixed value sets such as exit codes and status codes. Do not use groups of `public const int` fields for the same purpose.
- Never use `InternalsVisibleTo` for test assemblies anywhere in the solution. It is never necessary when production code is written correctly. If a test needs access, fix the production API: make the constructor, factory, type, or registration path public instead.
- Avoid code folders whose names repeat project or namespace segments and create confusing nested names.
- Place interfaces in an `Abstractions` folder beside their implementations.
- When the user asks to clean up code they added, fix the reported defect in place. Do not delete their abstraction, move its logic into `Program`, or replace it with a different design unless they explicitly ask.
- Avoid one-of designs: do not introduce parallel APIs, overloads, or shapes that mean "pick one of several ways to do the same thing." Prefer one clear path unless the user explicitly asks for alternatives.

## LTsoft Libraries

- Prefer classes, extension methods, and comparers from LTsoft NuGet packages (`LTs.Utils`, `LTs.Json`, `LTs.TestUtils`, `LTs.Web`, and related LTsoft packages) before introducing local helpers.
- Search existing LTsoft packages and sibling repositories for an existing utility before adding duplicate implementations.
- For `JToken` comparisons in tests, use `BeSameJsonAs` from `LTs.TestUtils.FluentAssertions` or `JTokenEqualityComparer` from `Newtonsoft.Json.Linq` with `BeEquivalentTo`, not custom comparers.
- For collection comparisons in tests, use `ContainExactlyEquivalent`, `ContainEquivalentSubset`, or `NotContainEquivalentInSubset` from `LTs.TestUtils.FluentAssertions`. Do not use `BeEquivalentTo` on collections.

## JSON

- Prefer `LTs.Json` extension methods for JSON parsing and serialization (`ParseAsJToken`, `ToJson`).
- `Newtonsoft.Json` (`JToken`, `JObject`, `JToken.FromObject`) is the underlying JSON model when `LTs.Json` does not provide a helper.
- Do not add `System.Text.Json` usage in new production code unless an existing component in the same area already depends on it.
- Prefer `JToken` for JSON payloads and schema metadata at tool and API boundaries.
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
- Use `<ProjectReference>` only to other projects in this repository under `src\`.
- Never add `<ProjectReference>` to projects outside this repository, including sibling repositories, absolute paths, or relative paths that escape the solution (for example `..\..\..\SomeOtherRepo\...`).
- Reference LTsoft libraries from other repositories only through published `<PackageReference>` entries.
- Do not duplicate `<PackageReference>` entries in `*.test` or `*.test.integration` projects when the same package is already referenced by a production project under test through `<ProjectReference>`.
- Never add `<InternalsVisibleTo>` in any project for test assemblies. Correctly designed production APIs make it unnecessary.
- Test projects should reference test-only packages (`LTs.TestUtils`, `xunit`, `Moq`, `coverlet.collector`, `Microsoft.NET.Test.Sdk`) plus `<ProjectReference>` to the project(s) under test.
- If a test needs behavior from a production dependency, exercise it through the production project's public API instead of adding that package to the test project.

## Testing Rules

- Add or update unit tests in the matching test project for every production code change in the same pass. Do not defer test coverage to a follow-up.
- Never use `InternalsVisibleTo` for test assemblies. Tests must exercise the public production API only.
- Each executable or library should have a matching test project. Do not place executable-specific tests in a sibling project's test assembly.
- Test classes must always derive from `LTs.TestUtils.Tests.BaseTest`.
- Tests must always use FluentAssertions.
- Tests must use `BeEquivalentTo` when validating a single result or object.
- Do not use `BeEquivalentTo` on collections. Use `ContainExactlyEquivalent`, `ContainEquivalentSubset`, or `NotContainEquivalentInSubset` from `LTs.TestUtils.FluentAssertions` instead.
- When an object contains a collection property, assert the object with `BeEquivalentTo` (excluding the collection when needed) and assert the collection separately with the appropriate collection extension.
- Helper classes added only for tests must be internal classes under `<test project>\Infrastructure`, in their own file, not nested inside the test class.
- Group test methods in `#region` blocks named after the production method or API under test (for example `#region LoadConfiguration`, `#region AddConfiguration`).
- Do not insert a blank line immediately after a `#region` declaration or immediately before its matching `#endregion`.
- For composition entry points such as `Program.RegisterConfiguration` and `Program.RegisterServices`, add tests for both configuration registration and service registration.
- Service registration tests must build the real registration path, resolve services from the container, and verify both resolved dependencies and configuration values.
- Build test configuration with `AddJsonString()` from `LTs.Configurations.Extensions` instead of `AddInMemoryCollection()` when exercising configuration binding.
- Keep Arrange, Act, and Assert as separate blocks in each test.
- Assign parsed, transformed, or extracted values to a local variable before asserting on them.
- The expression before `.Should()` must be a local variable or parameter name only. Do not chain method calls, property access, indexing, LINQ, casts, or other expressions directly into `.Should()`.
- Do not insert a blank line between a variable assignment and the assertion that follows it.
- Insert a blank line between separate assertion groups in the Assert block so each check reads as its own unit.
- Use FluentAssertions call style: `subject.Should().Be...()` or `collection.Should().ContainExactlyEquivalent(...)`. Do not add `ShouldBe...()` methods on the subject itself.
- Prefer combined FluentAssertions checks such as `NotBeNullOrEmpty()` instead of separate `NotBeNull()` and `NotBeEmpty()` (or `NotBeNullOrWhiteSpace()`) on the same subject.
- Add custom FluentAssertions extensions on the assertions object (for example `JTokenAssertions`), not on the subject type.
- Keep expected objects visible in the test method. Write the full expected shape inline in `BeEquivalentTo`, `ContainExactlyEquivalent`, or equivalent assertions.
- Do not hide expected results behind helpers such as `CreateExpectedResult()` or `BuildExpected...()`. Use `Infrastructure/` helpers for setup, normalization, deserialization, or shared assertion options only.
- Do not assign the expected object to a local variable such as `var expected = ...` when it is only used by `BeEquivalentTo` or `ContainExactlyEquivalent`.
- Keep scenario literals inline in tests. Do not introduce test-level constants whose only purpose is to hide readable scenario text or expected values.
- Prefer one `BeEquivalentTo` on the whole result over property-by-property assertion chains when validating a single object outcome.

## Git And Release Notes

- When completing a set of changes, always provide the commit comment text in the format below, even if the user does not explicitly ask for it.
- Read the current milestone `## Commit comments` section in `_ignore\Notes\Issue <n> - Milestone <n>.md` for subject prefix, tone, and bullet style when that file exists. Match that section; do not invent format from examples alone.
- Never create, edit, or delete files under `_ignore\Notes\`. The user maintains those files.
- Append commit bullet lines to the active release notes file under `ReleaseNotes\` when one exists.
- Use the LTsoft release notes style: title `# Version X.Y.Z`, one `## Changes` section, and a flat bullet list starting with imperative verbs (`Add`, `Update`, `Refactor`, and so on).
- Do not add milestone, area, or subsection headings to release notes files.
- Do not create a new `ReleaseNotes\vX.Y.Z.md` file or update `README.md` change logs until the user explicitly starts a new version release.
- Do not stage or commit files unless the user explicitly asks.

## Git Commit Messages

- Before drafting commit text, run `git status --short` and `git diff` and include every uncommitted file in the working tree, including untracked files.
- Commit text must describe all non-staged and unstaged changes together; do not limit bullets to the latest conversation slice or the files touched in the current turn.
- Use this subject format: `<issue> - <concise summary>.`
- Use the issue number for the subject prefix, not the milestone number. On branches named `<issue>-milestone-<milestone>`, use `<issue>`. In `_ignore\Notes\Issue <n> - Milestone <m>.md`, use `<n>`. When unsure, read recent commits on the current branch and match their prefix.
- Start the summary with an imperative verb (for example: `Add`, `Update`, `Fix`, `Refactor`, `Introduce`).
- Keep the subject on one line and end it with a period.
- Put `- ` bullet lines immediately after the subject. Do not add a blank line between the subject and the first bullet.
- Use `- ` bullet lines for notable changes in the current commit only; keep bullets short and specific.
- Describe only the changes in the current commit. Do not repeat bullets for work already committed on the branch.
- Match the tone and structure of the current milestone `## Commit comments` section and recent commits on the branch when available.

Example:

```text
42 - Wire host planner through typed host configuration.
- Remove legacy planner options and pass host configuration through planner composition.
- Fix configuration registration to honor the sectionName parameter.
- Update planner and composition tests for host configuration.
```

## Verification

- Restore after package or project changes.
- Build the solution after code or project changes.
- Run the narrowest relevant tests for the change.
- For integration tests that depend on local services, run them only when they are relevant or explicitly requested.
- Before final response, verify edited files have CRLF line endings and no empty lines at EOF.
- Report any tests not run and why.
