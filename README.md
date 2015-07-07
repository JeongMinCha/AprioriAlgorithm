# This Project is...
This project is for 1st assignment of Data Mining Course held at Hanyang University, Seoul, South Korea. The purpose of this project is to **find association rules using the Apriori algorithm**.



## Requirements
#### Execution
- execute the program with three arguments
- **minimum support, input file, output file**
- e.g) apriori.exe 50 input.txt output.txt


#### Input file format
[item_id]\t[item_id]\n
[item_id]\t[item_id]\t[item_id]\t[item_id]\t[item_id]\n
[item_id]\t[item_id]\t[item_id]\t[item_id]\n

- Row: a transaction


#### Output file format
[item_set]\t[associative_item_set]\t[support(%)]\t[confidence(%)]\n
[item_set]\t[associative_item_set]\t[support(%)]\t[confidence(%)]\n 

- [item_set]\t[associative_item_set]: association rules with minimum support- Support: probability that a transaction contains [item_set] [associative_item_set]
- Confidence: conditional probability that a transaction having [item_set] also contains [associative_item_set]

