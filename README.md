# .Net cleaner CLI tool

You can use this simple CLI tool for deleting temp folders from existing dotnet solution folders.

The list of temp directories which are deleted by this tool:

- `/node_modules/`
- `/bin/`
- `/obj/`
- `/.vs/`
- `/build/`

---
### How to run

1. You can open the `.exe` file
   1. Also You can call it through console `../dotnetCleaner.exe`
2. Pass arguments
   1. You can use `-h` or `--help` for getting the list of supported commands
   2. For cleaning - pass `-p {root directory path}`

---
### Attention
There is a chance that this tool can delete important files. So, it firstly displays all pathes that it found for deletion - **PLEASE REVIEW IT!** And everything is okay - approve deletion.