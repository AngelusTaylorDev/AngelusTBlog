// Tag list
let index = 0;

// Add A new Tag
function AddTag() {

    // Get the tag Entry Element
    var tagEntry = document.getElementById("TagEntry");

    // Using the search Function
    let searchResult = Search(tagEntry.value);
    if (searchResult != null) {

        // Trigger the sweet alert for the empty string
        swalWithBootstrapButtons.fire({
            html: `<span class='font-weight-bolder'>${searchResult}</span>`
        })
    }
    else {

        // Creating a new Select option
        let newOption = new Option(tagEntry.value, tagEntry.value);

        // Getting the tag list
        document.getElementById("TagList").options[index++] = newOption;
    }

    // clear out the tag entry area
    tagEntry.value = "";
    return true;
}


// Delete A Tag
function DeleteTag() {
    let tagCount = 1;
    let tagList = document.getElementById("TagList");
    if (!tagList) {
        return false;
    }

    if (tagList.selectedIndex == -1) {
        swalWithBootstrapButtons.fire({
            html: '<span class="font-weight-bolder">SELECT A TAG BEFORE DELETING</span>'
        });
        return true;
    }

    while (tagCount > 0) {
        if (tagList.selectedIndex >= 0) {
            tagList.options[tagList.selectedIndex] = null;
            --tagCount;
        }
        else
            tagCount = 0;
        index--;
    }
}

// Add the tags to the Database
$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
})


// Look for tagValues variable to see if it has data
if (tagValues != '') {
    let tagArray = tagValues.split(",");
    for (let loop = 0; loop < tagArray.length; loop++) {
        // Load or replace current options
        ReplaceTag(tagArray[loop], loop);
        index++;
    }
}

// Replace tag function
function ReplaceTag(tag, index) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[index] = newOption;
}

// Search function will detect a empty or duplicate tag. 
// Return a error string if detected.
function Search(str) {

    if (str == "") {
        return "Empty Tags are not allowed";
    }

    let tagsElement = document.getElementById("TagList");

    if (tagsElement) {
        let options = tagsElement.options;
        for (let index = 0; index < options.length; index++) {
            if (options[index].value == str) {
                return `The tag #${str} is a Duplicate and not allowed`;
            }
        }
    }
}

// Sweet Alert Swal
const swalWithBootstrapButtons = Swal.mixin({

    customClass: {
        confirmButton: 'btn btn-outline-danger btn-lg btn-block'
    },
    imageUrl: "/img/oops.png",
    imageWidth: '150px',
    timer: 3000,
    buttonsStyling: false
});