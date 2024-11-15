backend_scheme=$(printenv BACKEND_SCHEME)
backend_authority=$(printenv BACKEND_AUTHORITY)

sed -i "s|http|$backend_scheme|" /usr/share/nginx/html/appsettings.json
sed -i "s|localhost:5203|$backend_authority|" /usr/share/nginx/html/appsettings.json