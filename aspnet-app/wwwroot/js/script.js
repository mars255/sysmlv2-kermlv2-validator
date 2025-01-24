// Global variables to store statistics
let numLines = 0;
let numCharacters = 0;
let numNonSpaceCharacters = 0;
let numWords = 0;


document.addEventListener('DOMContentLoaded', () => {
    const textarea = document.getElementById('inputString');
    textarea.addEventListener('input', updateLineNumbers);
    textarea.addEventListener('scroll', () => {
        document.querySelector('.line-numbers').scrollTop = textarea.scrollTop;
    });

    // Initialize line numbers on load
    updateLineNumbers();

    // Add event listener for the "Stats" button
    document.getElementById('statsButton').addEventListener('click', () => {
        const inputString = document.getElementById('inputString').value;
        collectStringStatistics(inputString);  // Collect stats
        displayStats();  // Display the stats in the modal
    });

    // Add event listener for closing the modal
    document.querySelector('.close').addEventListener('click', closeModal);

    // Close the modal if the user clicks anywhere outside of it
    window.addEventListener('click', (event) => {
        const modal = document.getElementById('statsModal');
        if (event.target === modal) {
            closeModal();
        }
    });
});

// Function to update the line numbers as the user adds line breaks
function updateLineNumbers() {
    const textarea = document.getElementById('inputString');
    const lineNumbers = document.querySelector('.line-numbers');
    const lineCount = textarea.value.split('\n').length;
    const lines = Array.from({ length: lineCount }, (_, i) => `<div></div>`).join('');
    lineNumbers.innerHTML = lines;
}

// Sends JSON requests to the server with the correct format
async function sendRequest() {
    const inputString = document.getElementById('inputString').value;
    const parserType = document.getElementById('parserType').value; // Get the selected parser type

    // Collect and print string statistics
    collectStringStatistics(inputString);

    const requestBody = {
        input_string: inputString,
        parser_type: parseInt(parserType) // Ensure the parser type is an integer
    };

    console.log('Sending request:', requestBody); // Log the request body

    const response = await fetch('/Home/Parse', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
    });

    const data = await response.json();
    displayResult(data.result, data.success);
}

// Function to display result 
function displayResult(resultText, success) {
    const resultBox = document.getElementById('result');
    resultBox.textContent = resultText;

    // Reset styles
    resultBox.style.padding = '10px';
    resultBox.style.borderRadius = '5px';
    resultBox.style.fontSize = '1.2em';
    resultBox.style.fontWeight = 'bold';
    resultBox.style.color = '#000000';
    resultBox.style.textAlign = 'center';

    // Apply conditional styling
    if (!success) {
        resultBox.style.backgroundColor = '#ffcccc'; // Error: Light Red background
    } else if (resultText.includes('error')) {
        resultBox.style.backgroundColor = '#fff3b3'; // Warning: Light Yellow background
    } else {
        resultBox.style.backgroundColor = '#a5d6a7'; // Success: Light Green background
    }
}

// Function to collect statistics about the input string
function collectStringStatistics(inputString) {
    numLines = inputString.split('\n').length;
    numCharacters = inputString.length;
    numNonSpaceCharacters = inputString.replace(/\s/g, '').length;
    numWords = inputString.trim().split(/\s+/).length;
}

// Function to display the stats in the modal
function displayStats() {
    const statsDisplay = document.getElementById('statsDisplay');
    statsDisplay.innerHTML = `
        <strong>Lines:</strong> ${numLines}<br>
        <strong>Characters:</strong> ${numCharacters}<br>
        <strong>Non-space Characters:</strong> ${numNonSpaceCharacters}<br>
        <strong>Words:</strong> ${numWords}
    `;
    // Show the modal
    const modal = document.getElementById('statsModal');
    modal.style.display = 'block';
}

// Function to close the modal
function closeModal() {
    const modal = document.getElementById('statsModal');
    modal.style.display = 'none';
}