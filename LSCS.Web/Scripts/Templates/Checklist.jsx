var Checklist = React.createClass({
    retrieveChecklistFromServer: function() {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function() {
            var data = JSON.parse(xhr.responseText);
            this.setProps(data[0]); //Needs to be set to the single checklist to remove the [0] hardcoded indexing necessary on props
            var surveyLoc = data[0].SurveyLocation;
        }.bind(this);
        xhr.send();
    },
    componentDidMount: function() {
        this.retrieveChecklistFromServer();
        window.setInterval(this.retrieveChecklistFromServer, this.props.pollInterval);
    },
    getInitialState: function() {
        return {data: []};
    },
    render: function() {
        var styles = {
            //Page styles, not sure how nesting works yet, call in html via "style={styles}"
        };

        var address = this.props.SurveyLocation ? this.props.SurveyLocation['Address'] : '';
        var landDistrict = this.props.SurveyLocation ? this.props.SurveyLocation['LandDistrict'] : '';


        //Need to set hdr-address to left justify when it can't fit with hdr-dates
        return (
			<div>
				<div class="checklist-hdr">
					<h2 class="checklist-hdr-title" style={{borderBottom:'solid 1px'}}>#{this.props['FileNumber']} - {this.props['Title']}</h2>
                    <div class="checklist-hdr-dates" style={{display:'inline-block'}}>
                        <h4>Created on: {this.props['CreatedAt']}</h4>
                        <h4>Last modified: {this.props['LastModified']}</h4>
                        <h4>Land District: {landDistrict['Name']}</h4>
                    </div>
                    <div class="checklist-hdr-address" style={{display:'inline-block', float:'right'}}>
                        <address>
                            {address['AddressLine1']}<br/>
                            {address['City']}, {address['PostalCode']}<br/>
                            {address['StateProvince']}, {address['CountryRegion']}<br/>
                        </address>
                    </div>
				</div>
				<br/>
				<div style={{clear:'both'}}>
					<p>{this.props['Description']}</p>

				</div>
			</div>
        );
		
    }
});

React.render(
    <Checklist url="http://localhost:1059/api/checklists" pollInterval={2000} />,
    document.getElementById('checklist-view')
);