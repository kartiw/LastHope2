CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `lasthope`@`%` 
    SQL SECURITY DEFINER
VIEW `lasthope`.`view_partner_area` AS
    SELECT 
        `lasthope`.`partner`.`partnerid` AS `partnerid`,
        `lasthope`.`partner`.`partnername` AS `partnername`,
        `lasthope`.`partner`.`middlename` AS `middlename`,
        `lasthope`.`partner`.`lastname` AS `lastname`,
        `lasthope`.`partner`.`regular` AS `regular`,
        `lasthope`.`partner`.`active` AS `active`,
        `lasthope`.`partner`.`donor` AS `donor`,
        `lasthope`.`partner`.`retreat` AS `retreat`,
        `lasthope`.`partner`.`fullfamily` AS `fullfamily`,
        `lasthope`.`partner`.`gender` AS `Gender`,
        `lasthope`.`partner`.`dateofenrollment` AS `dateofenrollment`,
        `lasthope`.`partner`.`birthdate` AS `birthdate`,
        `lasthope`.`partner`.`baptismdate` AS `baptismdate`,
        `lasthope`.`partner`.`weddingdate` AS `weddingdate`,
        `lasthope`.`partner`.`deathdate` AS `deathdate`,
        `lasthope`.`partner`.`inactivedate` AS `inactivedate`,
        `lasthope`.`partner`.`dateofmembershipeligibility` AS `dateofmembershipeligibility`,
        `lasthope`.`partner`.`biometricid` AS `biometricid`,
        `lasthope`.`partner`.`biometriccardnumber` AS `biometriccardnumber`,
        `lasthope`.`partner`.`address` AS `address`,
        `lasthope`.`partner`.`state` AS `state`,
        `lasthope`.`partner`.`country` AS `country`,
        `lasthope`.`partner`.`email` AS `email`,
        `lasthope`.`partner`.`mobileno` AS `mobileno`,
        `lasthope`.`partner`.`phoneno` AS `phoneno`,
        `lasthope`.`partner`.`referencepartnerid` AS `referencepartnerid`,
        `lasthope`.`partner`.`followedid1` AS `followedid1`,
        `lasthope`.`partner`.`followedid2` AS `followedid2`,
        `lasthope`.`partner`.`followedid3` AS `followedid3`,
        `lasthope`.`partner`.`followedid4` AS `followedid4`,
        `lasthope`.`partner`.`visitremarks` AS `visitremarks`,
        `lasthope`.`area`.`AreaId` AS `AreaId`,
        `lasthope`.`area`.`AreaName` AS `AreaName`,
        `lasthope`.`area`.`City` AS `City`,
        `lasthope`.`area`.`Pincode` AS `Pincode`,
        `lasthope`.`area`.`Region` AS `Region`
    FROM
        (`lasthope`.`partner`
        LEFT JOIN `lasthope`.`area` ON ((`lasthope`.`partner`.`areaid` = `lasthope`.`area`.`AreaId`)))



CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `lasthope`@`%` 
    SQL SECURITY DEFINER
VIEW `lasthope`.`prayer_request_view` AS
SELECT CONCAT(p1.partnername, ' ', p1.middlename, ' ', p1.lastname) as request_by, 
		CONCAT(p2.partnername, ' ', p2.middlename, ' ', p2.lastname) as requested_for, 
        r.* 
FROM lasthope.request r
LEFT JOIN lasthope.partner p1
	ON r.partnerid = p1.partnerid
LEFT JOIN lasthope.partner p2
	ON r.relationid = p2.partnerid