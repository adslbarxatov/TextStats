# TextStats: user guide
> **ƒ** &nbsp;RD AAOW FDL; 24.11.2024; 19:05



### Page contents

- [General information](#general-information)
- [Supported characters](#supported-characters)
- [Hotkeys](#hotkeys)
- [Download links](https://adslbarxatov.github.io/DPArray#textstats)
- [Версия на русском языке](https://adslbarxatov.github.io/TextStats/ru)

---

### General information

This tool obtains statistics on the specified text. It allows you to collect next data:
- Quantity of every letter in the text (in descending order)
- Quantity of every digit (in descending order)
- Quantity of every character (in descending order)
- Percentage of letters, digits and characters
- Quantity of words with their lengths (in descending order)
- Maximum, minimum and average word lengths
- Quantity of sentences with their lengths in letters and words (in descending order)
- Maximum, minimum and average lengths of sentences
- Quantity of paragraphs with their lengths in letters, words and sentences (in descending order)
- Maximum, minimum and average lengths of paragraphs

The text can be loaded from file under UTF8, UTF16 and CP1251 encodings (auto-detection by
preamble). Statistics can also be saved to file.

&nbsp;



### Supported characters

This app now detects only next characters as letters when considering words,
sentences and paragraphs. Other characters will be treated as separators. Let us know if you
need to change this behavior.

- Russian alphabet:

```
АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ
абвгдеёжзийклмнопрстуфхцчшщъыьэюя
```

- English alphabet:

```
ABCDEFGHIJKLMNOPQRSTUVWXYZ
abcdefghijklmnopqrstuvwxyz
```

- Digits:

```
0123456789
```
- Connecting characters:

```
-'’
```

&nbsp;



### Hotkeys

For Windows:
- `Ctrl` + `O`: open text file
- `Ctrl` + `S`: save statistics to file
- `Ctrl` + `F`: search for specified text
- `Ctrl` + `G`: get statistics for entered text again
- `Ctrl` + `B`: get text from clipboard
