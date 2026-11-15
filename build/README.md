# NuGet build scripts

## Introduction
PowerShell scripts in this folder build release packages, copy them to a local feed defined in `nuget.config`, and publish them to GitHub Packages.


## Setup GitHub token
- Go to [GitHub Developer Settings](https://github.com/settings/tokens).
- Click **Generate new token**, then **Generate new token (classic)**.
- Under **Note**, enter a name for the token.
- Select an **Expiration** date (up to one year).
- Select these scopes:
  - `read:packages`
  - `write:packages`
  - `delete:packages`
  - `repo`
- Click **Generate token**.
- Copy the token and store it securely. You cannot view it again after you leave the page.
- Click **Configure SSO** and authorize the organization that hosts the packages.


## Setup nuget.config
Create or update `nuget.config` at the root of your repositories folder (for example `D:\Repositories\nuget.config`). Add a GitHub Packages source, a `localNuGets` folder source, and credentials for GitHub:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="githubLTsoft" value="https://nuget.pkg.github.com/LTsoft-repo/index.json" />
    <add key="localNuGets" value="./_localNuGets" />
  </packageSources>

  <packageSourceCredentials>
    <githubLTsoft>
      <add key="Username" value="@YourUserName@" />
      <add key="ClearTextPassword" value="@YourGithubApiKey@" />
    </githubLTsoft>
  </packageSourceCredentials>
</configuration>
```

Replace `@YourUserName@` with your GitHub username and `@YourGithubApiKey@` with the token from the previous section. The `localNuGets` path is resolved relative to the folder that contains `nuget.config`.


## Usage
| Script                       | Purpose                                                                 |
|------------------------------|-------------------------------------------------------------------------|
| `BuildRelease.ps1`           | Clean, build the solution, and pack `.nupkg` files into `build\NuGet`.  |
| `CopyToLocalLibrary.ps1`     | Copy packages from `build\NuGet` into the `localNuGets` feed.           |
| `PublishNuGetPackages.ps1`   | Push packages from `build\NuGet` to the `githubLTsoft` NuGet source.    |

Run the scripts from this folder in that order when you want a local feed and a remote publish. `CopyToLocalLibrary.ps1` walks up from `build` to locate `nuget.config`; it does not require administrator privileges.
