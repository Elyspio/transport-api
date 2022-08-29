db.createView("Location", "Location.Regions", [{
	$lookup: {
		from: "Location.Departements", let: {
			region_id: "$_id",
		}, pipeline: [{
			$match: {
				$expr: {
					$eq: ["$RegionId", "$$region_id"],
				},
			},
		}, {
			$lookup: {
				from: "Location.Cities", let: {
					"departement_id": "$_id",
				}, pipeline: [{
					$match: {
						$expr: {
							$eq: ["$DepartementId", "$$departement_id"],
						},
					},
				}], "as": "Cities",
			},
		}], "as": "Departements",
	},
},

	{
		$unset: ["Departements.RegionId", "Departements.Cities.DepartementId"],
	}]);