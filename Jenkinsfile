pipeline {
    agent any

    environment {
        DOCKER_HUB_CREDENTIALS = credentials('dockerhub-credentials')
        DOCKER_HUB_USERNAME    = 'abhiranjan12'
        IMAGE_TAG              = "${BUILD_NUMBER}"
    }

    stages {

        stage('Checkout') {
            steps {
                echo 'Pulling latest code from GitHub...'
                checkout scm
            }
        }

        stage('Build - AuthService') {
            steps {
                bat 'dotnet restore AuthService.API/AuthService.API.csproj'
                bat 'dotnet build AuthService.API/AuthService.API.csproj --configuration Release'
            }
        }

        stage('Build - PolicyService') {
            steps {
                bat 'dotnet restore PolicyService.API/PolicyService.API.csproj'
                bat 'dotnet build PolicyService.API/PolicyService.API.csproj --configuration Release'
            }
        }

        stage('Build - ClaimsService') {
            steps {
                bat 'dotnet restore ClaimsService.API/ClaimsService.API.csproj'
                bat 'dotnet build ClaimsService.API/ClaimsService.API.csproj --configuration Release'
            }
        }

        stage('Build - AdminService') {
            steps {
                bat 'dotnet restore AdminService.API/AdminService.API.csproj'
                bat 'dotnet build AdminService.API/AdminService.API.csproj --configuration Release'
            }
        }

        stage('Build - ApiGateway') {
            steps {
                bat 'dotnet restore ApiGateway/ApiGateway.csproj'
                bat 'dotnet build ApiGateway/ApiGateway.csproj --configuration Release'
            }
        }

        stage('Docker Build Images') {
            steps {
                echo 'Building Docker images...'
                bat "docker build -t %DOCKER_HUB_USERNAME%/smartsure-auth:%IMAGE_TAG% -f AuthService.API/Dockerfile ."
                bat "docker build -t %DOCKER_HUB_USERNAME%/smartsure-policy:%IMAGE_TAG% -f PolicyService.API/Dockerfile ."
                bat "docker build -t %DOCKER_HUB_USERNAME%/smartsure-claims:%IMAGE_TAG% -f ClaimsService.API/Dockerfile ."
                bat "docker build -t %DOCKER_HUB_USERNAME%/smartsure-admin:%IMAGE_TAG% -f AdminService.API/Dockerfile ."
                bat "docker build -t %DOCKER_HUB_USERNAME%/smartsure-gateway:%IMAGE_TAG% -f ApiGateway/Dockerfile ."
            }
        }

        stage('Docker Push to DockerHub') {
            steps {
                echo 'Pushing images to DockerHub...'
                bat "docker login -u %DOCKER_HUB_CREDENTIALS_USR% -p %DOCKER_HUB_CREDENTIALS_PSW%"

                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-auth:%IMAGE_TAG%"
                bat "docker tag %DOCKER_HUB_USERNAME%/smartsure-auth:%IMAGE_TAG% %DOCKER_HUB_USERNAME%/smartsure-auth:latest"
                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-auth:latest"

                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-policy:%IMAGE_TAG%"
                bat "docker tag %DOCKER_HUB_USERNAME%/smartsure-policy:%IMAGE_TAG% %DOCKER_HUB_USERNAME%/smartsure-policy:latest"
                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-policy:latest"

                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-claims:%IMAGE_TAG%"
                bat "docker tag %DOCKER_HUB_USERNAME%/smartsure-claims:%IMAGE_TAG% %DOCKER_HUB_USERNAME%/smartsure-claims:latest"
                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-claims:latest"

                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-admin:%IMAGE_TAG%"
                bat "docker tag %DOCKER_HUB_USERNAME%/smartsure-admin:%IMAGE_TAG% %DOCKER_HUB_USERNAME%/smartsure-admin:latest"
                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-admin:latest"

                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-gateway:%IMAGE_TAG%"
                bat "docker tag %DOCKER_HUB_USERNAME%/smartsure-gateway:%IMAGE_TAG% %DOCKER_HUB_USERNAME%/smartsure-gateway:latest"
                bat "docker push %DOCKER_HUB_USERNAME%/smartsure-gateway:latest"
            }
        }

        stage('Deploy - Run Containers') {
            steps {
                echo 'Stopping old containers...'
                bat "docker-compose down"

                echo 'Starting new containers...'
                bat "docker-compose up -d"

                echo 'Containers are running!'
            }
        }

        stage('Verify') {
            steps {
                echo 'Waiting for services to start...'
                bat "timeout /t 15"
                bat "docker ps"
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed! Services are running.'
            echo 'Gateway: http://localhost:5000/swagger'
        }
        failure {
            echo 'Pipeline failed. Check logs above.'
        }
        always {
            bat "docker logout"
        }
    }
}
