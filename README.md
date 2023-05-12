# Predmetni projekat iz pretmeta USI

Članovi tima:
* Vladislav Radović SV27-2021
* Mihajlo Vujisić SV26-2021
* Nikola Mitrović SV18-2021
* Miloš Milanović SV32-2021

# Conventions
Team project conventions

### Commit messages
[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
Use a category name for categorizing commits, followed by a colon and a description of the commit message.

Example categories:
- `feat` - feature
- `fix` - bug fix
- `test` - tests
- `refactor` - refactoring code
- `docs` - documentation
- etc.

Format: `category: description`

### Branch names
Branch names should include a category, reference and description.

Example categories:
- `feature` for a new feature branch
- `bugfix` for a bug fixing branch
- `test` for testing branch
- etc.

Format: `git branch category/reference/kebab-case-description`

Example: `git branch feature/issue-15/issue-prescription`

### CRUD method names
Use these method names for CRUD operations:
1. **Create** for creating a new entity
2.  **Get** for retrieving the entity
3. **Update** for updating an existing entity with new values
4.  **Delete** for deleting an entity
