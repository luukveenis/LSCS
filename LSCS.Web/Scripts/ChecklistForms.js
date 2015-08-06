$(function () {

    /* Since HTML forms don't natively supported nested structures
     * like we require for Mongo, the form submission is interrupted
     * and the data converted into a suitable format before POSTing   */
    $("#new-checklist-form").submit(function (event) {
        event.preventDefault();
        var url = $(this).prop('action');

        // Take the flat fields in the HTML form and build the nested JSON structure
        var result = $(this).serializeArray().reduce(function (p, c) {
            var keys = c.name.split(".");
            p = $.extend(true, {}, p, constructJsonObject(keys, c.value));
            return p;
        }, {});
        result["Items"] = checklistItems();

        $.ajax({
            method: "POST",
            url: url,
            data: JSON.stringify(result),
            contentType: "application/json"
        })
        .done(function (msg) {
            window.location.replace("http://localhost:49177/checklists");
        })
        .fail(function (jqXHR, status, error) {
            alert("Request failed: " + status);
        })
    });

    /* Similar to creating a new checklist, we need to manually construct
     * nested JSON structure we require. In this case we have to take into
     * account the checklist items and create the array in the correct order. */
    $("#edit-checklist-form").submit(function (event) {
        event.preventDefault();
        var url = $(this).prop('action');

        var result = $(this).serializeArray().reduce(function (p, c) {
            var match; // stores the result of matching the Status regex
            if (match = c.name.match(/Items\[(\d+)\]\.Status/)) {
                var itemText = checklistItems()[parseInt(match[1])].Text;
                p["Items"].push({ Text: itemText, Status: c.value });
            } else {
                var keys = c.name.split(".");
                p = $.extend(true, {}, p, constructJsonObject(keys, c.value));
            }
            return p;
        }, { Items: [] }); // Begin with an empty Items array so we can append to it

        debugger;
        $.ajax({
            method: "PUT",
            url: url,
            data: JSON.stringify(result),
            contentType: "application/json"
        })
        .done(function (msg) {
            window.location.replace("http://localhost:49177/checklists");
        })
        .fail(function (jqXHR, status, error) {
            alert("Request failed: " + error);
        })
    });

    /* Takes a list of keys and a value and constructs the nested JSON structure.
     * i.e.: (["Foo", "Bar"], val1) => { "Foo": { "Bar": val1 } }  */
    function constructJsonObject(keys, value) {
        var result = {};
        result[keys[0]] =  keys.length > 1 ? constructJsonObject(keys.slice(1), value) : value;
        return result;
    }

    /* Since the form fields aren't customizable per the current requirements,
     * we add them to the form right before it's submitted since this avoids having
     * to create an array structure in an HTML form. */
    function checklistItems() {
        return [
            // Plan Title
            { Text: "Type of Plan" },
            { Text: "Legal Description & registered plan no." },
            { Text: "BCGS NO." },
            { Text: "Appropriate Scale & Bar, including intended plot size" },
            { Text: "Legend explaining all symbols and non-standard abbreviations" },
            { Text: "Bearing derivation and reference" },
            { Text: "Notation: bearings to BTs are magnetic or planned bearings North Point" },
            { Text: "North Point" },
            // Main Body of Plan
            { Text: "Appropriate designation for title or Interest parcels (e.g. Lot Number)" },
            { Text: "All essential dimensions given and closure calculated" },
            { Text: "Title & Interest Parcel Area or Volume correct & to required precision-GSI Rule 3" },
            { Text: "Boundaries reestablished and/or lots divided in accordance with Land Survey Act" },
            { Text: "Sufficient ties to evidence of previous surveys" },
            { Text: "Monumentation labelled and correct - GSI Rule 1-2 to 1-7" },
            { Text: "Read or “Lane” and name, when available, where road is being dedicated" },
            { Text: "Remember to check for hooked parcels, part parcels and remainders" },
            { Text: "New Dedicated Road or RW fully dimensioned with widths indicated-GSI Rule" },
            { Text: "No text less than 2mm" },
            { Text: "Plotting to scale and drafting legible - GSI Rule 3-2 & 3-3" },
            { Text: "Bold outline 1.0 - 1.5 mm centered on boundary (including any detail drawings)" },
            { Text: "Existing R/W, Easement or Covenant boundaries shown with broken lines - GSI Rule 3-4" },
            { Text: "Details of bearing trees and ancillary evidence found and made - GSI Rule 3-4" },
            { Text: "Radius, arc, radial bearings for each curve point - GSI Rule 3-4" },
            { Text: "Railway plan in un-surveyed land has district lot number assigned" },
            { Text: "Access to water body where applicable - LTA s75(1)" },
            { Text: "Label Un-surveyed Crown Land including theoretical or unsurveyed portions of townships" },
            // Scenery
            { Text: "Check status of adjacent roads. Have they all been dedicated?" },
            { Text: "Parcel boundaries (incl. highway, roads and railway) shown with solid lines - Rule 3-4(2)(g)" },
            { Text: "Description(s) given for all surrounding lands - GSI Rule 3-4(1)(r)" },
            { Text: "Primary parcel designations prominent in body of plan (use 'DL' not 'Lot') - Rule 10-14" },
            { Text: "Existing Road Names shown - GSI Rule 3-4" },
            { Text: "Roads, Trails, and Seismic Lines shown and labelled with width and posted as required" },
            { Text: "'Rem' added on lot and \"portion of\" or \"part of\" in title where appropriate" },
            // Deposit Statement
            { Text: "Plan lies within (Regional District) statement - GSI Rule 3-4" },
            { Text: "Leave 7 cm 12 cm clear space in top right corner for Registrar's notation pursuant to S 56 LTA" },
            // Integrated Survey Area
            { Text: "Grid bearing notation; ISA name and number, datum and bearing derivation - GSI Rule 5-7" },
            { Text: "Control monuments tied in accordance with GSI Rules 5-4(2)" },
            { Text: "Meets accuracy standards of integrated legal survey - GSI Rule 5-4 (3) & (4)" },
            { Text: "Control monuments shown on plan with required symbol and respective designation - GSI Rule 5-7(2)" },
            // Miscellaneous
            { Text: "Spelling check" },
            { Text: "Standard plan size - GSI Rule 3-1" },
            { Text: "If practical, top of plan orientated north - GSI Rule 3-3(5)" },
            { Text: "Notation regarding existing records that plan is compiled from." },
            // Electronic Plan
            { Text: "Plan Image created with Adobe 6.0 or higher with minimum 600 dpi resolution - GSI Rule 3-1 (1)" },
            { Text: "All plan features black ink on white background with no ornate fonts - GSI Rule 3-3(1)" },
            { Text: "No signatures on plan - GSI Rule 3-3(7)" },
            { Text: "Plan complies with all standards for electronic submissions approved by S.G. GSI Rule 3-3 (12)" }
        ]
    }
});