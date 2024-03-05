$(function () {
    $('#add-client-book').on('click', function (event) {

        let clientId = event.target.dataset.clientid;
        let $selectedBook = $("#select-book-element option:selected");
        let bookId = $selectedBook[0].dataset.bookid;
        $.ajax({
            url: "/Client/Book/Add",
            type: 'POST',
            data: {
                clientId: clientId,
                bookId: bookId
            },
            success: function (data) {
                loadClientBooks(event);
            },
            error: function (error) {
                console.log("error is " + error);
            }
        });
    });

    $('#select-book-element').on('click', function (event) {
        let $selectEl = $(event.target);
        let options = []
        
        $.ajax({
            url: "/Book/List",
            type: 'GET',
            success: function (data) {
                data.forEach(el => {
                    options.push(`<option data-bookid="${el.BookId}">${el.Name}</option>`)
                })
                $selectEl.empty();
                $selectEl.append(options.join("\n"));
            },
            error: function (error) {
                console.log("error is " + error);
            }
        });
    });

    $('#show-client-book-button').on('click', function (event) {
        let $clientBookViewer = $("#client-book-viewer");
        if ($clientBookViewer.children().length === 0)
            loadClientBooks(event);
        else
            $clientBookViewer.empty();
    });

    function loadClientBooks(event) {
        let clientId = event.target.dataset.clientid;
        $.ajax({
            url: "/Client/Book/ShowList",
            type: 'GET',
            data: {
                clientId: clientId
            },
            success: function (data) {
                let $clientBookViewer = $("#client-book-viewer");
                $clientBookViewer.empty();
                $clientBookViewer.append(data);
            },
            error: function (error) {
                console.log("error is " + error);
            }
        });
    }

    $(document.body).on('click', '.delete-client-book-button', function (event) {
        let clientId = event.target.dataset.clientid;
        let bookId = event.target.dataset.bookid;

        $.ajax({
            url: "/Client/Book/Delete",
            type: 'POST',
            data: {
                clientId: clientId,
                bookId: bookId
            },
            success: function (data) {
                loadClientBooks(event);
            },
            error: function (error) {
                console.log("error is " + error);
            }
        });
    });
})