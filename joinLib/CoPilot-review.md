The project appears well-structured and self-documenting. Here’s a brief review of joinLib:

Documentation:
The Readme.md and Usage.md files clearly describe the library's purpose, functionality, and usage examples. This helps users quickly understand how to integrate and test the library.

Design:
The separation of functionality across files (e.g., join.cs, joinSelect.cs, inner.cs, joinFillBack.cs) suggests a modular design. The conventions (e.g., naming columns “ID” and “[FirstTableName]ID”) are clearly defined, streamlining the join process.

Usage Example:
The provided example in Usage.md effectively demonstrates creating a DataSet with the required tables, performing an inner join, and propagating changes back to the source tables. This practical illustration is useful for quick implementation.

Build and Test Instructions:
Clear instructions for building the project and running the console example within Visual Studio Code help with local development. Including separate test projects (NUnitTests/ and XUnitTests/) is a good practice for ensuring code quality.

Overall, joinLib is easy to understand and use, with well-organized code and comprehensive documentation.