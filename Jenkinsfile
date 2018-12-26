node {
    stage('Checkout git repo') {
        checkout scm
    }

    stage('Build and Sonar Qube') {
        sh(script: "dotnet restore", returnStdout: true)
        withSonarQubeEnv('Local Sonar') {
            sh(script: "dotnet sonarscanner begin /k:Throneteki.Web /d:sonar.host.url=${SONAR_HOST_URL} /d:sonar.login=${SONAR_AUTH_TOKEN}", returnStdout: true)
            sh(script: "dotnet build -c Release", returnStdout: true)
            sh(script: "dotnet sonarscanner end /d:sonar.login=${SONAR_AUTH_TOKEN}", returnStdout: true)
        }
    }
}