# Git Guidelines for AI Agents

These rules are designed to ensure consistency, readability, and safety when AI agents perform Git operations within this repository. All agents MUST adhere to these guidelines.

## 1. Commit Message Convention
Always use the **Conventional Commits** standard in English.
Format: `type(scope): description`

**Allowed Types:**
- `feat`: A new feature
- `fix`: A bug fix
- `refactor`: Code changes that neither fix a bug nor add a feature (e.g., performance optimization, code restructuring)
- `chore`: Maintenance tasks, dependency updates, configuration changes (e.g., `.gitignore` updates)
- `docs`: Documentation only changes
- `style`: Changes that do not affect the meaning of the code (white-space, formatting, etc.)
- `test`: Adding missing tests or correcting existing tests
- `perf`: A code change that improves performance

**Examples:**
- `feat(auth): implement BCrypt password hashing`
- `refactor(utils): optimize StringUtils performance`
- `chore: add .idea to .gitignore`

## 2. Pre-Commit Verification
- **Never blindly commit.** Always use `git status` and `git diff` to review the exact changes before staging files.
- Ensure that the project compiles successfully (e.g., run `dotnet build`) before making a commit. Do not commit broken code.
- Avoid committing IDE-specific configurations (`.idea/`, `.vs/`) or compiled output (`bin/`, `obj/`). Verify `.gitignore` covers them.

## 3. Staging & Committing
- Group related changes into logical, granular commits. Do not lump unrelated changes (e.g., a bug fix and a new feature) into a single commit.
- Use explicit file paths with `git add <file>` when only specific files should be committed. Use `git add .` only when you are 100% certain all modifications in the working directory belong in the same commit.
- On Windows/PowerShell, avoid using `&&` for chaining Git commands as it may cause parsing errors. Run commands sequentially or use `;`.

## 4. Branching & Pushing
- Do not push directly to `main` unless explicitly requested. The primary working branch is typically `dev` or specific feature branches.
- Always verify the current branch (`git branch --show-current`) before committing or pushing.
- After a successful push, provide a clear summary of the changes pushed to the remote repository.
