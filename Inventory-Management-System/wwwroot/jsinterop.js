function preventFormSubmission(formId) {
    
    document.getElementById(`${formId}`).addEventListener('keydown', function(e)  {
        if (e.key === "Enter") {
            event.preventDefault();
            return false;
        }
    })
}