/*
 *
 * Front-end JS functions
 *
 * */
//Function to update the line numbers as the user adds line breaks
function updateLineNumbers() {
    const textarea = document.getElementById('inputString');
    const lineNumbers = document.querySelector('.line-numbers');

    const lineCount = textarea.value.split('\n').length;
    const lines = Array.from({ length: lineCount }, (_, i) => `<div></div>`).join('');
    lineNumbers.innerHTML = lines;
}

document.addEventListener('DOMContentLoaded', () => {
    const textarea = document.getElementById('inputString');
    textarea.addEventListener('input', updateLineNumbers);
    textarea.addEventListener('scroll', () => {
        document.querySelector('.line-numbers').scrollTop = textarea.scrollTop;
    });

    // Initialize line numbers on load
    updateLineNumbers();
});

// Sends Json requests to the server with the correct format
async function sendRequest() {
    const inputString = document.getElementById('inputString').value;
    const parserType = document.getElementById('parserType').value; // Get the selected parser type

    const response = await fetch('/parse', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            input_string: inputString,
            parser_type: parserType // Send the selected parser type
        })
    });

    const data = await response.json();

    if (data.success) {
        // Display the result in the scrollable result div
        displayResult(data.result);
    } else {
        document.getElementById('result').innerHTML = 'Error: ' + data.error.replace(/\n/g, '<br>');
    }
}

// Function to display result 
function displayResult(resultText) {
    const resultBox = document.getElementById('result');
    resultBox.textContent = resultText; 
}

