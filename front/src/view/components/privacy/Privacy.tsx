import React from "react";
import { Box, Grid, Link, ListItem, Paper, Typography } from "@mui/material";
import List from "@mui/material/List";

export function Privacy() {
	return (
		<Paper sx={{ maxHeight: "90%", overflowY: "auto" }}>
			<Box p={2}>
				<Grid container spacing={2} direction={"column"}>
					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Privacy Policy</Typography>
						<Typography variant={"body1"}>
							Jonathan Guichard built the ElyTransport app as
							a Free app. This SERVICE is provided by
							Jonathan Guichard at no cost and is intended for use as
							is.
						</Typography>
						<Typography noWrap={false}>
							This page is used to inform visitors regarding my
							policies with the collection, use, and disclosure of Personal
							Information if anyone decided to use my Service.
						</Typography>
						<Typography>
							If you choose to use my Service, then you agree to
							the collection and use of information in relation to this
							policy. The Personal Information that I collect is
							used for providing and improving the Service. I will not use or share your information with
							anyone except as described in this Privacy Policy.
						</Typography>
						<Typography>
							The terms used in this Privacy Policy have the same meanings
							as in our Terms and Conditions, which are accessible at
							ElyTransport unless otherwise defined in this Privacy Policy.
						</Typography>
					</Grid>
					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Information Collection and Use</Typography>

						<Typography>
							For a better experience, while using our Service, I require you to provide us your <strong>Precise
							Location</strong>.
						</Typography>
						<Typography>
							The app does use third-party services that may collect
							information used to identify you.
						</Typography>
						<Typography>
							Link to the privacy policy of third-party service providers used
							by the app
							<Link ml={1} href="https://www.google.com/policies/privacy/">Google Slay Services</Link>.
						</Typography>

					</Grid>
					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Log Data</Typography>
						<Typography>
							I want to inform you that whenever you
							use my Service, in a case of an error in the app
							I collect data and information (through third-party
							products) on your phone called Log Data. This Log Data may
							include information such as your device Internet Protocol
							(“IP”) address, device name, operating system version, the
							configuration of the app when utilizing my Service,
							the time and date of your use of the Service, and other
							statistics.
						</Typography>
					</Grid>
					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Cookies</Typography>
						<Typography>
							Cookies are files with a small amount of data that are
							commonly used as anonymous unique identifiers. These are sent
							to your browser from the websites that you visit and are
							stored on your device's internal memory.
						</Typography>
						<Typography>
							This Service does not use these “cookies” explicitly. However,
							the app may use third-party code and libraries that use
							“cookies” to collect information and improve their services.
							You have the option to either accept or refuse these cookies
							and know when a cookie is being sent to your device. If you
							choose to refuse our cookies, you may not be able to use some
							portions of this Service.
						</Typography>
					</Grid>

					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Service Providers</Typography>
						<Typography>
							I may employ third-party companies and
							individuals due to the following reasons:
						</Typography>
						<List>
							<ListItem>
								<Typography variant={"body2"}>To facilitate our Service</Typography>
							</ListItem>
							<ListItem>
								<Typography variant={"body2"}>To provide the Service on our behalf</Typography>
							</ListItem>
							<ListItem>
								<Typography variant={"body2"}>To perform Service-related services</Typography>
							</ListItem>
							<ListItem>
								<Typography variant={"body2"}>To assist us in analyzing how our Service is
									used</Typography>
							</ListItem>
						</List>
						<Typography>
							I want to inform users of this Service
							that these third parties have access to their Personal
							Information. The reason is to perform the tasks assigned to
							them on our behalf. However, they are obligated not to
							disclose or use the information for any other purpose.
						</Typography>
					</Grid>


					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Security</Typography>
						<Typography>
							I value your trust in providing us your
							Personal Information, thus we are striving to use commercially
							acceptable means of protecting it. But remember that no method
							of transmission over the internet, or method of electronic
							storage is 100% secure and reliable, and I cannot
							guarantee its absolute security.
						</Typography>
					</Grid>

					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Links to Other Sites</Typography>
						<Typography>
							This Service may contain links to other sites. If you click on
							a third-party link, you will be directed to that site. Note
							that these external sites are not operated by me.
							Therefore, I strongly advise you to review the
							Privacy Policy of these websites. I have
							no control over and assume no responsibility for the content,
							privacy policies, or practices of any third-party sites or
							services.
						</Typography>
					</Grid>

					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Children’s Privacy</Typography>
						<Typography>
							These Services do not address anyone under the age of 13.
							I do not knowingly collect personally
							identifiable information from children under 13 years of age. In the case
							I discover that a child under 13 has provided
							me with personal information, I immediately
							delete this from our servers. If you are a parent or guardian
							and you are aware that your child has provided us with
							personal information, please contact me so that
							I will be able to do the necessary actions.
						</Typography>
					</Grid>


					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Changes to This Privacy Policy</Typography>
						<Typography>
							I may update our Privacy Policy from
							time to time. Thus, you are advised to review this page
							periodically for any changes. I will
							notify you of any changes by posting the new Privacy Policy on
							this page.
						</Typography>
						<Typography>This policy is effective as of 2022-03-07.</Typography>
					</Grid>

					<Grid item>
						<Typography color={"primary"} variant={"overline"}>Contact Us</Typography>
						<Typography>
							If you have any questions or suggestions about my
							Privacy Policy, do not hesitate to contact me at <Link
							href={"mailto:elyspio.dev@gmail.com"}>elyspio.dev@gmail.com</Link>.
						</Typography>
						<Typography>This privacy policy page was created at
							<Link mx={1}
							      href={"https://privacypolicytemplate.net"}>https://privacypolicytemplate.net</Link>
							and modified/generated by <Link ml={1}
							                                href={"https://app-privacy-policy-generator.nisrulz.com"}>App
								Privacy
								Policy Generator</Link>.
						</Typography>
					</Grid>

				</Grid>
			</Box>

		</Paper>

	);
}

