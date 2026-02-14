# Publish NuGet packages

## Setup GitHub Token (API Key)
- Go to [GitHub Developer Settings](https://github.com/settings/tokens).
- Click on **Generate new token** and select **Generate new token (classic)**.
- Under **Note**, enter a name for the token.
- Select the **Expiration** date. It can be up to 1 year.
- Select the following scopes:
  - `read:packages`
  - `write:packages`
  - `delete:packages`
  - `repo`
- Click on **Generate token**.
- Copy the token and save it in a secure place. **You will not be able to see it again.**
- Click **Configure SSO**, and make sure to select the organization you want to use the token with.


## Setup
- If you don't have the `nuget.config` file, create it in the root of your repositories' folder.
  - Add the following content to the `nuget.config` file:
  ```xml
  <?xml version="1.0" encoding="utf-8"?>
    <configuration>
      <packageSources>
        <add key="githubLTsoft" value="https://nuget.pkg.github.com/LTsoft-repo/index.json" />
      </packageSources>
    
      <packageSourceCredentials>
    
        <githubLTsoft>
          <add key="Username" value="@YourUserName@" />
          <add key="ClearTextPassword" value="@YourGithubAPIKey@" />
        </githubLTsoft>
    
      </packageSourceCredentials>
    </configuration>
  ```
  - Replace `@YourUserName@`, and `@YourGithubAPIKey@` with your information.
    - `@YourUserName@`: Your GitHub username.
    - `@YourGithubAPIKey@`: The GitHub API key you created on the previous steps.

## Usage
- Run the `BuildRelease.ps1` script to build the solution.
- Run the `PublishNuGetPackages.ps1` script to publish the NuGet packages.